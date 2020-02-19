import { NgModule, ErrorHandler } from '@angular/core';
import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';
import { IonicStorageModule } from '@ionic/storage'
import { AgmCoreModule } from '@agm/core';
import { QRCodeModule } from 'angular2-qrcode';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { TextMaskModule } from 'angular2-text-mask';

import { MyApp } from './app.component';
import { ConfigurationSettings } from './app.config';
import { LojaPage } from '../pages/loja/loja';
import { LoginPage } from '../pages/login/login';
import { ClientePage } from '../pages/cliente/cliente';
import { RememberPasswordPage } from '../pages/remember-password/remember-password';
import { CartaoPage } from '../pages/cartao/cartao';
import { CartaoEditPage } from '../pages/cartao/cartaoEdit';
import { ProdutoPage } from '../pages/produto/produto';
import { ProdutoDetalhePage } from '../pages/produto/produtoDetalhe';
import { ListaCompraPage } from '../pages/listaCompra/listaCompra';
import { NovoPedidoPage } from '../pages/pedido/novoPedido';
import { DetalhePedidoPage } from '../pages/pedido/detalhePedido';
import { ListaPedidoPage } from '../pages/pedido/listaPedido';
import { Geolocation } from '@ionic-native/geolocation';
import {BarcodeScanner} from '@ionic-native/barcode-scanner';

import { Auth } from '../providers/auth';
import { Estabelecimento } from '../providers/estabelecimento';
import { Pessoa } from '../providers/pessoa';
import { Pagamento } from '../providers/pagamento';
import { Compra } from '../providers/compra';
import { StorageCtrl } from '../providers/storageCtrl';
import { Pedido } from '../providers/pedido';
import { Agendamento } from '../providers/agendamento';

@NgModule({
    declarations: [
        MyApp,
        LojaPage,
        LoginPage,
        ClientePage,
        RememberPasswordPage,
        CartaoPage,
        CartaoEditPage,
        ProdutoPage,
        ProdutoDetalhePage,
        ListaCompraPage,
        NovoPedidoPage,
        DetalhePedidoPage,
        ListaPedidoPage
    ],
    imports: [
        BrowserModule,
        IonicModule.forRoot(MyApp),
        IonicStorageModule.forRoot(),
        FormsModule,
        TextMaskModule,
        CommonModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyAsG_2D6FpW-j0TPZpe3da1UwDQHe5EtJY'
        }),
        QRCodeModule
    ],
    bootstrap: [IonicApp],
    entryComponents: [
        MyApp,
        LojaPage,
        LoginPage,
        ClientePage,
        RememberPasswordPage,
        CartaoPage,
        CartaoEditPage,
        ProdutoPage,
        ProdutoDetalhePage,
        ListaCompraPage,
        NovoPedidoPage,
        DetalhePedidoPage,
        ListaPedidoPage
    ],
    providers: [{
        provide: ErrorHandler, useClass: IonicErrorHandler
    },
        ConfigurationSettings, Auth, Estabelecimento, Pessoa, Pagamento, Compra, StorageCtrl, Pedido, Agendamento, Geolocation, BarcodeScanner
    ]
})
export class AppModule { }