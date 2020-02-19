// Import the config-related things
import { ConfigurationSettings } from '../app/app.config';

import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

import { StorageCtrl } from "../providers/storageCtrl";

import 'rxjs/add/operator/map';

@Injectable()
export class Compra {

    url: string;
    listaAtiva: any;

    constructor(public http: Http, public alertCtrl: AlertController, public storage: StorageCtrl, public toastCtrl: ToastController) {
        this.url = ConfigurationSettings.apiUrl;
    }

    getListaTemporaria(lojaId) {
        if (this.listaAtiva && this.listaAtiva.Loja.Id == lojaId) {
            return;
        }

        this.storage.get("claims").then((claims) => {
            let lista = {
                total: 0,
                pedido: null,
                usuario: null,
                loja: { Id: lojaId },
                itens: []
            };

            if (claims)
                lista.usuario = { Id: claims.UsuarioId };

            let headers = new Headers();
            headers.append('Content-Type', 'application/json');
            this.http.post(this.url + 'api/listacompra/', lista, { headers: headers })
                .subscribe(data => {
                    this.listaAtiva = data.json();
                },
                error => {
                    console.log("Error get lista sem usuário: " + error);
                    this.showTempToast(error.text());
                });
        });
    }

    atribuiListaUsuario(claims) {
        if (!this.listaAtiva)
            return;

        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        headers.append('Authorization', 'Bearer ' + claims["access_token"]);
        let options = new RequestOptions({ headers: headers });
        //this.listaAtiva.Usuario = { Id: claims["UsuarioId"] };

        //this.http.put(this.url + 'api/listacompra/' + this.listaAtiva.Id, this.listaAtiva, options)
        //    .subscribe(data => {
        //        this.listaAtiva = data.json();
        //    }, error => {
        //        console.log("Error: atribuiListaUsuario");
        //        this.showErrorAlert(error._body);
        //    });
    }

    getListaAtiva() :Promise<any> {

        //this.storage.get("claims").then((value) => {
            //let claims = value;
            let claims = this.storage.claims;
            if (this.listaAtiva && this.listaAtiva.Itens.length > 0) {
                this.atribuiListaUsuario(claims);
                return new Promise(resolve => { resolve(null) });
            }

            let headers = new Headers();
            //console.log(this.auth.claims);
            headers.append('Authorization', 'Bearer ' + claims.access_token);
            //headers.append('Content-Type', 'application/x-www-form-urlencoded');
            let options = new RequestOptions({ headers: headers });

            return new Promise(resolve => {
                this.http.get(this.url + 'api/listacompra/usuario/' + claims.UsuarioId, options)
                    .subscribe(data => {
                            let lista = data.json();
                            if (lista != null)
                                this.listaAtiva = lista;
                            resolve(this.listaAtiva);
                        },
                        error => {
                            console.log("Error get lista ativa: " + error);
                            this.showTempToast(error.text());
                        });
            });
        //});
    }

    getListaAtualizada() {
        let headers = new Headers();
        let options = new RequestOptions({ headers: headers });
        return new Promise(resolve => {
            this.http.get(this.url + 'api/listacompra/' + this.listaAtiva.Id, options)
                .subscribe(data => {
                    let json = data.json();
                    this.listaAtiva = json;
                    resolve(json);
                },
                error => {
                    console.log("Error get lista ativa: " + error);
                    this.showTempToast(error.text());
                });
        });
    }

    atualizarLista() {
        let headers = new Headers();
        //console.log(this.auth.claims);
        //let authToken = this.auth.claims;
        //headers.append('Authorization', 'Bearer ' + authToken);
        headers.append('Content-Type', 'application/x-www-form-urlencoded');
        let options = new RequestOptions({ headers: headers });

        return new Promise(resolve => {
            this.http.get(this.url + 'api/listacompra/' + this.listaAtiva.Id + '/atualizar', options)
                .subscribe(data => {
                    let json = data.json();
                    this.listaAtiva = json;
                    resolve(json);
                }, error => {
                    console.log("Error atualizar lista: " + error);
                    this.showTempToast(error.text());
                });
        });
    }

    adicionarItem(item) {
        if (!item || !item.Quantidade || isNaN(item.Quantidade) || item.Quantidade < 1) {
            this.showTempToast('Por favor informe uma quantidade válida.');
            return new Promise(resolve => { resolve(null) });
        }

        let headers = new Headers();
        this.http.post(this.url + 'api/listacompra/' + this.listaAtiva.Id + '/adicionaritem', item, { headers: headers })
            .subscribe(data => {
                this.listaAtiva = data.json();
                this.showTempToast("Item adicionado à lista de compras.");
            }, error => {
                this.showErrorAlert(error);
                this.showTempToast(error.text());
            });
    }

    alterarItem(item) {
        if (!item || !item.Quantidade || isNaN(item.Quantidade) || item.Quantidade < 1) {
            this.showTempToast('Por favor informe uma quantidade válida.');
            return new Promise(resolve => { resolve(null) });
        }

        let headers = new Headers();
        return new Promise((resolve, reject) => {
            this.http.post(this.url + 'api/listacompra/' + this.listaAtiva.Id + '/alteraritem', item, { headers: headers })
                .subscribe(data => {
                    let json = data.json();
                    this.listaAtiva = json;
                    resolve(json);
                }, error => {
                    console.log("Error alterar item: " + error);
                    this.showTempToast(error.text());
                    reject(error.text());
                });
        });
    }

    removerItem(item) {
        let headers = new Headers();
        //console.log(this.auth.claims);
        //let authToken = this.auth.claims;
        //headers.append('Authorization', 'Bearer ' + authToken);
        //headers.append('Content-Type', 'application/x-www-form-urlencoded');
        //let options = new RequestOptions({ headers: headers });

        return new Promise(resolve => {
            this.http.post(this.url + 'api/listacompra/' + this.listaAtiva.Id + '/removeritem', item, { headers: headers })
                .subscribe(data => {
                    let json = data.json();
                    this.listaAtiva = json;
                    resolve(json);
                }, error => {
                    console.log("Error remover item: " + error);
                    this.showTempToast(error.text());
                });
        });
    }

    showAlert(title, subtitle) {
        let alert = this.alertCtrl.create({
            title: title,
            subTitle: subtitle,
            buttons: ['OK']
        });
        alert.present();
    }

    showErrorAlert(message) {
        let alert = this.alertCtrl.create({
            title: 'Um erro ocorreu.',
            subTitle: message,
            buttons: ['OK']
        });
        alert.present();
    }

    showSuccessAlert(message) {
        let alert = this.alertCtrl.create({
            title: 'Sucesso',
            subTitle: message,
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