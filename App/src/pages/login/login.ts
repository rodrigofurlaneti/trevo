import { Component } from '@angular/core';
import { NavController, AlertController, NavParams } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

import { LoadingController } from 'ionic-angular';

import { LojaPage } from '../loja/loja';
import { ClientePage } from '../cliente/cliente';
import { RememberPasswordPage } from '../remember-password/remember-password';
import { NovoPedidoPage } from '../pedido/novoPedido';

import { Auth } from '../../providers/auth';

@Component({
    selector: 'page-login',
    templateUrl: 'login.html'
})
export class LoginPage {

    loader: any;
    redirect: any;

    usercreds = {
        name: '',
        password: ''
    };

    constructor(public navCtrl: NavController, public loadingCtrl: LoadingController, public alertCtrl: AlertController, public auth: Auth, public toastCtrl: ToastController, public navParams: NavParams) {
        this.redirect = navParams.get("redirect");
    }


    ionViewDidLoad() {
        console.log('ionViewDidLoad LoginPage');
    }

    login(user) {

        this.presentLoading();

        console.log('called login');
        this.usercreds = user;
        console.log('usercreds.name: ' + this.usercreds.name);
        console.log('usercreds.password: ' + this.usercreds.password);

        this.auth.authenticate(this.usercreds).then(data => {
            if (data !== null) {
                if (this.redirect && this.redirect === 'FINALIZAR_COMPRA') {
                    this.navCtrl.setRoot(NovoPedidoPage);
                }
                else {
                    this.navCtrl.setRoot(LojaPage);    
                }
            }
        });

        this.loader.dismiss();
    }

    presentLoading() {

        this.loader = this.loadingCtrl.create({ content: "Por favor aguarde..." });
        this.loader.present();

    }

    showAlert() {
        let alert = this.alertCtrl.create({
            title: 'Usuário ou Senha inválida',
            subTitle: 'Não foi encontrado o usúario ou senha!',
            buttons: ['OK']
        });
        alert.present();
    }

    rememberPassword() {
        this.navCtrl.setRoot(RememberPasswordPage);
    }

    dadosPessoais() {
        if (this.redirect) {
            this.navCtrl.setRoot(ClientePage, { redirect: this.redirect });
        } else {
            this.navCtrl.setRoot(ClientePage);
        }
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
