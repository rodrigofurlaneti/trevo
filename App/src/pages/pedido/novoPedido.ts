import { Component } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { NavController, NavParams, AlertController, Events } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

import { Auth } from '../../providers/auth';
import { Pagamento } from '../../providers/pagamento';
import { Compra } from '../../providers/compra';
import { Pedido } from '../../providers/pedido';
import { Agendamento } from '../../providers/agendamento';
 
import { LoginPage } from '../login/login';
import { CartaoPage } from '../cartao/cartao';
import { CartaoEditPage } from '../cartao/cartaoEdit';
import { DetalhePedidoPage } from '../pedido/detalhePedido';

@Component({
    selector: 'page-pedido',
    templateUrl: 'novoPedido.html'
})
export class NovoPedidoPage {

    cartoes: any;
    agendamentos: any;
    pedido: any;
    listaCompra: any;

    constructor(public http: Http, public navCtrl: NavController, public navParams: NavParams, public alertCtrl: AlertController, public auth: Auth, public pagamento: Pagamento, public events: Events, public compra: Compra, public pedidos: Pedido, public agendamentosProvider: Agendamento, public toastCtrl: ToastController) {

        this.cartoes = this.auth.pessoaLogada.Cartoes;

        //this.events.subscribe('cartao:added', (cartao) => {
        //    this.cartoes.push(cartao);
        //});

        this.pedido = {
            ListaCompra: this.compra.listaAtiva,
            Cartao: { },
            Agendamento: { },
            Usuario: { id: this.auth.claims.UsuarioId }
        };

        this.getAgendamentos();
    }
    
    showAlert(title, subtitle) {
        let alert = this.alertCtrl.create({
            title: title,
            subTitle: subtitle,
            buttons: ['OK']
        });
        alert.present();
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

    confirmarCompra() {
        this.pedidos.finalizaPedido(this.pedido).then(result => {
            this.pedido.Id = result["Id"];
            this.pedido.Status = result["Status"];
            this.pedido.QrCode = result["QrCode"];
            this.pedido.Motivo = result["Motivo"];
            this.pedido.Cartao = result["Cartao"];
            this.pedido.Agendamento = result["Agendamento"];
            this.pedido.DataInsercao = result["DataInsercao"];
            
            if (this.pedido.Status === 6) {
                this.showTempToast(this.pedido.Motivo);
                return;
            }

            this.showTempToast('Compra realizada com sucesso!');
            this.compra.listaAtiva = null;
            let params = { pedido: this.pedido };
            this.navCtrl.setRoot(DetalhePedidoPage, params);
        }, err => {
            this.showTempToast(err.text());
        });
    }

    editar(cartao) {
        var params = {};
        if (cartao)
            params["cartao"] = cartao;

        this.navCtrl.push(CartaoEditPage, params);
    }

    remover(cartao) {
        this.pagamento.remover(cartao).then(result => {
            this.showTempToast('Cartao removido com sucesso!');
            var index = this.cartoes.indexOf(cartao);
            this.cartoes.splice(index, 1);
        }, err => {
            this.showTempToast(err.text());
        });
    }

    getAgendamentos() {
        this.agendamentosProvider.getAgendamentos().then(result => {
            this.agendamentos = result;
        }, err => {
            this.showTempToast(err.text());
        });
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