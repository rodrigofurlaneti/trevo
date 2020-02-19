import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { LoadingController, AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

import { ProdutoPage } from "../produto/produto";
import { LoginPage } from '../login/login';
import { NovoPedidoPage } from '../pedido/novoPedido';
import { LojaPage } from '../loja/loja';
//import { } from '../loja/loja';

import { Compra } from '../../providers/compra';
import { Auth } from '../../providers/auth';

@Component({
    selector: 'page-listaCompra',
    templateUrl: 'listaCompra.html'
})
export class ListaCompraPage {

    listaCompra: any;
    loader: any;

    constructor(public navCtrl: NavController, private loadingCtrl: LoadingController, public navParams: NavParams, public compra: Compra, public auth: Auth, public alertCtrl: AlertController, public toastCtrl: ToastController) {
    }

    ionViewDidEnter() {
        this.listaCompra = this.compra.listaAtiva;
    }

    removeItem(item) {
        this.compra.removerItem(item).then(result => {
            this.listaCompra = result;
        });
    }

    alterarItem(item, acao) {

        let originalValue = item.Quantidade;

        if (acao === 'menos') {
            if (item.Quantidade <= 1)
                return;
            item.Quantidade--;
        } else if (acao === 'mais') {
            item.Quantidade++;
        } else if (acao === 'blur') {
            // não faz nada pois o cara digitou o valor
        } else {
            return;
        }

        this.presentLoading();
        this.compra.alterarItem(item).then(result => {
            if (result != null)
                this.listaCompra = result;
            else
                this.compra.getListaAtualizada().then(result => { this.listaCompra = result; });
            //
            this.loader.dismiss();
        }).catch((ex) => {
            item.Quantidade = originalValue;
            this.loader.dismiss();
        });
        
    }

    continuarComprando() {
        //if (this.listaCompra) {
        //    let params = {
        //        loja: this.listaCompra.Loja
        //    };
        //    this.navCtrl.push(ProdutoPage, params);
        //} else {
        //    this.navCtrl.push(ProdutoPage);
        //}
        this.navCtrl.setRoot(LojaPage);
    }

    finalizarCompra() {
        if (!this.auth.claims) {
            this.showTempToast('Cadastre-se ou logue-se para finalizar a compra.');
            this.navCtrl.setRoot(LoginPage, { redirect: 'FINALIZAR_COMPRA' });
            return;
        }

        // TODO: Encaminhar para escolha do cartão
        this.navCtrl.push(NovoPedidoPage);
    }

    presentLoading() {
        this.loader = this.loadingCtrl.create({ content: "Por favor aguarde..." });
        this.loader.present();
    }

    showAlert(title, subtitle) {
        let alert = this.alertCtrl.create({
            title: title,
            subTitle: subtitle,
            buttons: ['OK']
        });
        alert.present();
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