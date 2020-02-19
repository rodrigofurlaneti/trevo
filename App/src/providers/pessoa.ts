// Import the config-related things
import { ConfigurationSettings } from '../app/app.config';

import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { ToastController } from 'ionic-angular';

import { Auth } from '../providers/auth';

import 'rxjs/add/operator/map';

@Injectable()
export class Pessoa {

    headers: any;
    options: any;
    url: string = null;

    constructor(public http: Http, public auth: Auth, public toastCtrl: ToastController) {
        this.url = ConfigurationSettings.apiUrl;
        this.options = new RequestOptions();
    }

    doHeaders(withAuth) {
        if (withAuth) {
            let authToken = this.auth.claims.access_token;
            this.headers = new Headers();
            this.headers.append('Authorization', 'Bearer ' + authToken);
            this.headers.append('Content-Type', 'application/json');
            this.options.headers = this.headers;
        } else {
            this.headers = new Headers();
            this.headers.append('Content-Type', 'application/json');
        }
    }

    getPessoa(id) {
        this.doHeaders(true);
        return this.http.get(this.url + 'api/usuario/' + id, this.options).map(rs => rs.json());
    }

    getPaises() {
        this.doHeaders(false);
        return this.http.get(this.url + 'api/pais/', this.options).map(rs => rs.json());
    }

    getEstados() {
        this.doHeaders(false);
        return this.http.get(this.url + 'api/Estado/', this.options).map(rs => rs.json());
    }

    getCidades(estadoId) {
        this.doHeaders(false);
        return this.http.get(this.url + 'api/Cidade/getCidadesByEstados/' + estadoId, this.options).map(rs => rs.json());
    }

    salvar(usuario) {
        this.doHeaders(false);
        return new Promise(resolve => {
            this.http.post(this.url + 'api/usuario/account/register', usuario, { headers: this.headers })
                .subscribe(data => {
                    resolve(data.json());
                }, error => {
                    this.showTempToast(error.text());
                });
        });
    }

    update(pessoa) {
        this.doHeaders(true);
        var p = {
            Id: pessoa.Id,
            Nome: pessoa.Nome,
            DataNascimento: pessoa.DataNascimento,
            Sexo: pessoa.Sexo,
            Contatos: [],
            Documentos: [],
            Endereco: {}
        };

        if (pessoa.Contatos !== null) {
            p.Contatos = pessoa.Contatos;
        }

        if (pessoa.Documentos !== null) {
            p.Documentos = pessoa.Documentos;
        }

        if (pessoa.Enderecos !== null) {
            p.Endereco = pessoa.Endereco;
        }
        
        console.log(p);

        return new Promise(resolve => {
            this.http.post(this.url + 'api/pessoa/update/' + this.auth.pessoaLogada.Id, p, this.options)
                .subscribe(data => {
                    resolve(data.json());
                }, error => {
                    this.showTempToast(error.text());
                });
        });
    }

    senha(usuario) {
        //todo: update senha do usuario
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