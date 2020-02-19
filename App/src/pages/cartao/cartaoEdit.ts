import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, Events } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { Directive } from 'ionic2-text-mask'

import { Auth } from '../../providers/auth';
import { Pagamento } from '../../providers/pagamento';

@Component({
    selector: 'page-cartao',
    templateUrl: 'cartaoEdit.html'
})
export class CartaoEditPage {

    cartao: any;
    tempValidade: any;
    cvvMask: any;
    numeroMask: any;
    validadeMask: any;

    constructor(public navCtrl: NavController, public navParams: NavParams, public alertCtrl: AlertController, public auth: Auth, public pagamento: Pagamento, public events: Events, public toastCtrl: ToastController) {

        this.setMascaras();

        var c = navParams.get("cartao");
        if (c) {
            this.cartao = c;
            this.tempValidade = c.Validade;
        } else {
            this.cartao = {
                Numero: '',
                Validade: '',
                Cvv: ''
            };
            this.tempValidade = '';
        }
        //let result = this.pagamento.getCartao();
        //console.log('result cartao: ' + result);
    }

    setMascaras() {
        this.numeroMask = [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, ' ', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, ' ', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, ' ', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/];
        this.validadeMask = [/[0-1]/, /[0-9]/, '/', /[0-9]/, /[0-9]/];
        this.cvvMask = [/[0-9]/, /[0-9]/, /[0-9]/];
    }

    salvar(cartao) {
        cartao.Pessoa = { Id: this.auth.pessoaLogada.Id };
        cartao.Numero = cartao.Numero.replace(/[^0-9]/g, '');
        cartao.Validade = this.stringToDate(this.tempValidade);
        if (!cartao.Id)
            this.pagamento.salvar(cartao).then(result => {
                this.showTempToast('Cartão cadastrado com sucesso.');
                cartao.Validade = this.tempValidade;
                cartao.Id = result;
                this.events.publish('cartao:added', cartao);
                this.navCtrl.pop();
            }, err => {
                this.showTempToast(err.text());
            });
        else {
            this.pagamento.update(cartao).then(result => {
                this.showTempToast('Cartão atualizado com sucesso.');
                cartao.Validade = this.tempValidade;
                this.navCtrl.pop();
            }, err => {
                this.showTempToast(err.text());
            });
        }
    }

    stringToDate(str) {
        if (str === "")
            return str;
        //
        return str.substring(0, 3) + '20' + str.substring(3);
    }

    ionViewDidLoad() {
        console.log('ionViewDidLoad CartaoEditPage');
    }

    //showAlert(title, subtitle) {
    //    let alert = this.alertCtrl.create({
    //        title: title,
    //        subTitle: subtitle,
    //        buttons: ['OK']
    //    });
    //    alert.present();
    //}

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