// Import the config-related things
import { ConfigurationSettings } from '../app/app.config';

import { Injectable, EventEmitter } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { NativeStorage } from 'ionic-native';
import 'rxjs/add/operator/map';
import { ToastController, Events } from 'ionic-angular';

//import { Storage } from '@ionic/storage';

import { Compra } from "../providers/compra";
import { StorageCtrl } from "../providers/storageCtrl";

@Injectable()
export class Auth {

    public userLogged = new EventEmitter();
    claims: any;
    pessoaLogada: any;
    url: string = null;

    constructor(public http: Http, public storage: StorageCtrl, public listaCompra: Compra, public toastCtrl: ToastController, public events: Events) {
        this.url = ConfigurationSettings.apiUrl;
        this.http = http;

        this.events.subscribe('cartao:added', (cartao) => {
            this.pessoaLogada.Cartoes.push(cartao);
        });
    }

    authenticate(usercreds) {
        // TODO: MOCK PARA TESTE - REMOVER --------------------
        //usercreds.name = 'leandrogrando@leojarts.com.br';
        //usercreds.password = '123';
        let isPersistent = false; //é o lembrar-me
        //----------------------------------------------

        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded');

        let data = 'grant_type=password&username=' + usercreds.name + '&password=' + usercreds.password + '&IsPersistent=' + isPersistent;

        return new Promise(resolve => {
            this.http.post(this.url + 'token', data, { headers: headers })
                .subscribe(data => {
                    this.claims = data.json();
                    this.storage.claims = data.json();
                    this.storage.set('claims', this.claims).then(() => {
                        this.getPessoaLogada().then(p => {
                            this.listaCompra.getListaAtiva().then(data => {
                                this.userLogged.emit(this.claims);
                                resolve(this.claims);
                            });
                        });
                    });

                }, error => {
                    this.showTempToast(error.text());
                });
        });
    }

    rememberPassword(usercreds) {

        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let data = "\"" + usercreds.name + "\"";
        console.log(data);
        return new Promise(resolve => {
            this.http.post(this.url + 'api/usuario/forgotpassword', data, { headers: headers })
                .subscribe(data => {
                    resolve(data);
                    console.log("rememberPassword: " + data);
                }, error => {
                    console.log("Error: rememberPassword");
                    this.showTempToast(error.text());
                });
        });

    }

    getPessoaLogada() {

        if (!this.claims) {
            console.log('Usuário não logado');
            return new Promise(resolve => { resolve(null) });
        }

        let headers = new Headers();
        console.log(this.claims);
        let authToken = this.claims;
        headers.append('Authorization', 'Bearer ' + authToken.access_token);
        headers.append('Content-Type', 'application/x-www-form-urlencoded');
        let options = new RequestOptions({ headers: headers });
        var id = authToken.PessoaId;
        return new Promise(resolve => {
            this.http.get(this.url + 'api/pessoa/' + id, options)
                .subscribe(data => {
                    this.pessoaLogada = data.json();
                    resolve(this.pessoaLogada);
                }, error => {
                    console.log("Error: Ao obter pessoa logada.");
                    this.showTempToast(error.text());
                });
        });


        //return new Promise(resolve => {
        //    this.http.get(this.url + 'api/pessoa/' + id, options)
        //        .subscribe(data => {
        //            console.log(data.json());
        //            resolve(data.json());
        //        }, error => {
        //            console.log("Error: Dados Pessoais");
        //        });
        //});
    }

    showTempToast(message) {
        let text = "";

        if (/^[\],:{}\s]*$/.test(message.replace(/\\["\\\/bfnrtu]/g, '@')
            .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
            .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {
            var json = JSON.parse(message);
            if (json.isTrusted) {
                text = "Um erro de conexão ocorreu. Tente novamente.";
            } else if (json.error_description) {
                text = json.error_description;
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
