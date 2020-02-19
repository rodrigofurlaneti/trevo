import { Component } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { NavController, NavParams, AlertController, Events } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

import { Auth } from '../../providers/auth';
import { Pagamento } from '../../providers/pagamento';
import { Compra } from '../../providers/compra';
import { Pedido } from '../../providers/pedido';
import { LojaPage } from '../loja/loja';

import { LoginPage } from '../login/login';
import { DetalhePedidoPage } from '../pedido/detalhePedido';

@Component({
    selector: 'page-pedido',
    templateUrl: 'listaPedido.html'
})
export class ListaPedidoPage {

    listaPedidos: any;

    constructor(public http: Http, public navCtrl: NavController, public navParams: NavParams, public alertCtrl: AlertController, public auth: Auth, public pagamento: Pagamento, public events: Events, public compra: Compra, public pedidos: Pedido, public toastCtrl: ToastController) {

        if (!this.auth.claims) {
            this.showTempToast('Cadastre-se ou logue-se para ver seus pedidos.');
            this.navCtrl.setRoot(LoginPage);
            return;
        }
    }

    showAlert(title, subtitle) {
        let alert = this.alertCtrl.create({
            title: title,
            subTitle: subtitle,
            buttons: ['OK']
        });
        alert.present();
    }

    ionViewDidEnter() {
        this.getPedidos();
    }

    getPedidos() {
        this.pedidos.getPedidos().then(result => {
            this.listaPedidos = result;
        },
        err => {
            this.showTempToast(err.text());
        });
    }

    detalhes(pedido) {
        let params = { pedido: pedido };
        this.navCtrl.push(DetalhePedidoPage, params);
    }

    continuarComprando() {
        this.navCtrl.setRoot(LojaPage);
    }

    showTempToast(message) {
        let text = "";
        if (/^[\],:{}\s]*$/.test(message.replace(/\\["\\\/bfnrtu]/g, '@')
            .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
            .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {
            var json = JSON.parse(message);
            if (json.isTrusted) {
                text = "Um erro de conexão ocorreu. Tente novamente.";
            } else {
                text = message;
            }
        } else {
            text = message;
        }

        const toast = this.toastCtrl.create({
            message: text,
            duration: 4000,
            position: 'bottom',
            //showCloseButton: true,
            //closeButtonText: 'Ok'
        });
        //toast.onDidDismiss(this.dismissHandler);
        toast.present();
    }
}