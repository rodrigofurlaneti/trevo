import { Component } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { NavController, NavParams, AlertController, Events } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

import { Auth } from '../../providers/auth';
import { Pagamento } from '../../providers/pagamento';

import { LoginPage } from '../login/login';
import { CartaoEditPage } from '../cartao/cartaoEdit';

@Component({
    selector: 'page-cartao',
    templateUrl: 'cartao.html'
})
export class CartaoPage {

    cartoes: any;

    constructor(public http: Http, public navCtrl: NavController, public navParams: NavParams, public alertCtrl: AlertController, public auth: Auth, public pagamento: Pagamento, public events: Events, public toastCtrl: ToastController) {

        if (!this.auth.claims) {
            this.showTempToast('Cadastre-se ou logue-se para ver os cartões.');
            this.navCtrl.setRoot(LoginPage);
            return;
        }

        this.cartoes = [];
        this.getCartoes();

        //this.events.subscribe('cartao:added', (cartao) => {
        //    this.cartoes.push(cartao);
        //});
    }

    ionViewDidLoad() {
        console.log('ionViewDidLoad CartaoPage');
    }

    getCartoes() {
        this.cartoes = this.auth.pessoaLogada.Cartoes;
    }

    editar(cartao) {
        var params = {};
        if (cartao)
            params["cartao"] = cartao;

        this.navCtrl.push(CartaoEditPage, params);
    }

    remover(cartao) {
        this.pagamento.remover(cartao).then(result => {
            this.showTempToast('Cartão removido com sucesso.');
            var index = this.cartoes.indexOf(cartao);
            this.cartoes.splice(index, 1);
        }, err => {
            this.showTempToast(err.text());
        });
    }

    detectCardType(number) {
        var re = {
            electron: /^(4026|417500|4405|4508|4844|4913|4917)\d+$/,
            maestro: /^(5018|5020|5038|5612|5893|6304|6759|6761|6762|6763|0604|6390)\d+$/,
            dankort: /^(5019)\d+$/,
            interpayment: /^(636)\d+$/,
            unionpay: /^(62|88)\d+$/,
            visa: /^4[0-9]{12}(?:[0-9]{3})?$/,
            mastercard: /^5[1-5][0-9]{14}$/,
            amex: /^3[47][0-9]{13}$/,
            diners: /^3(?:0[0-5]|[68][0-9])[0-9]{11}$/,
            discover: /^6(?:011|5[0-9]{2})[0-9]{12}$/,
            jcb: /^(?:2131|1800|35\d{3})\d{11}$/
        }

        for (var key in re) {
            if (re[key].test(number)) {
                return key;
            }
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