import { Component } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { NavController, NavParams, AlertController, Events } from 'ionic-angular';

import { Auth } from '../../providers/auth';
import { Pagamento } from '../../providers/pagamento';
import { Compra } from '../../providers/compra';
import { Pedido } from '../../providers/pedido';

@Component({
    selector: 'page-pedido-lista',
    templateUrl: 'detalhePedido.html'
})
export class DetalhePedidoPage {

    pedido: any;
    
    constructor(public http: Http, public navCtrl: NavController, public navParams: NavParams, public alertCtrl: AlertController, public auth: Auth, public pagamento: Pagamento, public events: Events, public compra: Compra, public pedidos: Pedido) {

        this.pedido = navParams.get("pedido");
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
}