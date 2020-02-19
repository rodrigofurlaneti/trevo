import { Component } from '@angular/core';
import { NavController, NavParams, LoadingController } from 'ionic-angular';

import { ToastController } from 'ionic-angular';

import { CartaoPage } from '../cartao/cartao';
import { LojaPage } from '../loja/loja';
import { NovoPedidoPage } from '../pedido/novoPedido';

import { Pessoa } from '../../providers/pessoa';
import { Auth } from '../../providers/auth';


@Component({
    selector: 'page-cliente',
    templateUrl: 'cliente.html'
})
export class ClientePage {

    paises: any;
    estados: any;
    cidades: any;
    paisSelected: any;
    estadoSelected: any;
    cidadeSelected: any;
    loader: any;
    usuario: any;
    documento: any;
    contato: any;
    endereco: any;
    logado: boolean = true;
    cepMask: any;
    celularMask: any;
    nascimentoMask: any;
    cpfMask: any;
    redirect: any;

    constructor(public navCtrl: NavController, public navParams: NavParams, public loadingCtrl: LoadingController, public auth: Auth, public pessoa: Pessoa, public toastCtrl: ToastController) {

        this.redirect = navParams.get("redirect");

        this.setMascaras();
        this.presentLoading();
        this.createModels();

        this.pessoa.getPaises()
            .subscribe(data => {
                this.paises = data;
            });

        this.pessoa.getEstados()
            .subscribe(data => {
                this.estados = data;
            });

        this.cidades = null;
        
        if (this.auth.claims != null) {
            this.logado = false;
            console.log('usuario id by auth.claims: ' + this.auth.claims.UsuarioId);
            this.pessoa.getPessoa(this.auth.claims.UsuarioId)
                .subscribe(data => {
                    console.log('segue data pos subscribe ');
                    console.log(data.Pessoa.DataNascimento);
                    var dtnascimento = this.formatDate(data.Pessoa.DataNascimento);
                    console.log(dtnascimento)
                    this.usuario.Pessoa = data.Pessoa;
                    this.usuario.Id = data.Id;
                    this.maper(data.Pessoa);
                    console.log(this.usuario.Pessoa);
                    this.usuario.Pessoa.DataNascimento = dtnascimento;
                });
        }
        this.loader.dismiss();
    }

    formatDate(inputDate) {
        var date = new Date(inputDate);
        
        if (!isNaN(date.getTime())) {
            var m = date.getMonth() + 1;
            var ms = m.toString();
            ms = ms[1] ? ms : "0" + ms[0];
            var d = date.getDate().toString();
            d = d[1] ? d : "0" + d[0];
            return d + '/' + ms + '/' + date.getFullYear();
        }
    }

    setMascaras() {
        this.cpfMask = [/[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];
        this.nascimentoMask = [/[0-3]/, /[0-9]/, '/', /[0-1]/, /[0-9]/, '/', /[0-2]/, /[0-9]/, /[0-9]/, /[0-9]/];
        this.cepMask = [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/, /[0-9]/];
        this.celularMask = ['(', /[0-9]/, /[0-9]/, ')', ' ', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/];
    }

    salvar() {

        this.presentLoading();

        if (this.usuario.Id !== "") {
            let p = {
                Pessoa: {
                    Id: this.usuario.Pessoa.Id,
                    Nome: this.usuario.Pessoa.Nome,
                    DataNascimento: this.usuario.Pessoa.DataNascimento,
                    Sexo: this.usuario.Pessoa.Sexo,
                    Documentos: [
                        {
                            Numero: this.documento.Cpf,
                            Tipo: 2,
                            Id: this.documento.CpfId
                        }
                    ],
                    Contatos: [
                        {
                            Email: this.contato.Email,
                            Tipo: 1,
                            Id: this.contato.EmailId
                        }, {
                            Numero: this.contato.Celular,
                            Tipo: 3,
                            Id: this.contato.CelularId
                        }
                    ],
                    Endereco:
                    {
                        Id: this.endereco.Id,
                        Tipo: 1,
                        Cep: this.endereco.Cep,
                        Logradouro: this.endereco.Logradouro,
                        Numero: this.endereco.Numero,
                        Complemento: this.endereco.Complemento,
                        Bairro: this.endereco.Bairro,
                        Cidade: {
                            Id: this.cidadeSelected,
                            Estado: {
                                Id: this.estadoSelected,
                                Pais: {
                                    Id: this.paisSelected
                                }
                            }
                        }
                    }
                }
            };
            this.pessoa.update(p.Pessoa).then(u => {
                this.navCtrl.setRoot(LojaPage);
                this.loader.dismiss();
            });
        }
        else {
            if (this.usuario.Senha !== this.usuario.ConfirmarSenha) {
                this.showTempToast('Senha e Confirmar senha não confere.');
                return;
            }
            if (this.usuario.Senha.length < 3) {
                this.showTempToast('Senha deve ter mais de 3 caracteres.');
                return;
            }
            
            let u = {
                Pessoa: {
                    Nome: this.usuario.Pessoa.Nome,
                    DataNascimento: this.usuario.Pessoa.DataNascimento,
                    Sexo: this.usuario.Pessoa.Sexo,
                    Documentos: [
                        {
                            Numero: this.documento.Cpf,
                            Tipo: 2
                        }
                    ],
                    Contatos: [
                        {
                            Email: this.contato.Email,
                            Tipo: 1
                        }, {
                            Numero: this.contato.Celular,
                            Tipo: 3
                        }
                    ],
                    Endereco:
                    {
                        Tipo: 1,
                        Cep: this.endereco.Cep,
                        Logradouro: this.endereco.Logradouro,
                        Numero: this.endereco.Numero,
                        Complemento: this.endereco.Complemento,
                        Bairro: this.endereco.Bairro,
                        Cidade: {
                            Id: this.cidadeSelected,
                            Estado: {
                                Id: this.estadoSelected,
                                Pais: {
                                    Id: this.paisSelected
                                }
                            }
                        }
                    }
                },
                Senha: this.usuario.Senha
            };
            //
            this.pessoa.salvar(u).then(s => {
                let usercreds = {
                    name: this.contato.Email,
                    password: this.usuario.Senha
                };

                this.auth.authenticate(usercreds).then(data => {
                    if (data !== null) {
                        if (this.redirect && this.redirect === 'FINALIZAR_COMPRA') {
                            this.navCtrl.setRoot(NovoPedidoPage);
                        } else {
                            this.showTempToast("Adicione um cartão para completar seu cadastro.");
                            this.navCtrl.setRoot(CartaoPage);
                        }
                        this.loader.dismiss();
                    }
                });
            }).catch((ex) => {
                this.loader.dismiss();
            });
        }
    }

    createModels() {
        this.paisSelected = 0;
        this.estadoSelected = 0;
        this.cidadeSelected = 0;
        this.usuario = {
            Id: "",
            Pessoa: {
                Id: 0,
                Nome: "",
                DataNascimento: "",
                Sexo: "",
                Documentos: [
                    {
                        Numero: "",
                        Tipo: 2,
                        Id: 0
                    }
                ],
                Contatos: [
                    {
                        Email: "",
                        Tipo: 1,
                        Id: 0
                    }, {
                        Numero: "",
                        Tipo: 3,
                        Id: 0
                    }
                ],
                Endereco:
                {
                    Id: 0,
                    Tipo: 1,
                    Cep: "",
                    Logradouro: "",
                    Numero: "",
                    Complemento: "",
                    Bairro: "",
                    Cidade: {
                        Id: 0,
                        Descricao: "",
                        Estado: {
                            Id: 0,
                            Descricao: "",
                            Pais: {
                                Id: 0,
                                Descricao: ""
                            }
                        }
                    }
                }
            },
            Senha: "",
            ConfirmarSenha: ""
            
        };
        this.documento = { Cpf: "", CpfId: 0 };
        this.contato = { Email: "", Celular: "", EmailId: 0, CelularId: 0 };
        this.endereco = {
            Id: 0,
            Cep: "",
            Logradouro: "",
            Numero: "",
            Complemento: "",
            Bairro: "",
            Cidade: "",
            Estado: "",
            Pais: "",
            CidadeId: 0,
            EstadoId: 0,
            PaisId: 0
        };
    }

    maper(pessoa) {
        this.documento = { Cpf: pessoa.Documentos.find(x => x.Tipo == 2).Numero, CpfId: pessoa.Documentos.find(x => x.Tipo == 2).Id };
        this.contato = {
            Email: pessoa.Contatos.find(x => x.Tipo == 1).Email,
            Celular: pessoa.Contatos.find(x => x.Tipo == 3).Numero,
            EmailId: pessoa.Contatos.find(x => x.Tipo == 1).Id,
            CelularId: pessoa.Contatos.find(x => x.Tipo == 3).Id
        }
        this.endereco = {
            Id: pessoa.Endereco.Id,
            Cep: pessoa.Endereco.Cep,
            Logradouro: pessoa.Endereco.Logradouro,
            Numero: pessoa.Endereco.Numero,
            Complemento: pessoa.Endereco.Complemento,
            Bairro: pessoa.Endereco.Bairro,
            Cidade: pessoa.Endereco.Cidade.Descricao,
            CidadeId: pessoa.Endereco.Cidade.Id,
            Estado: pessoa.Endereco.Cidade.Estado.Descricao,
            EstadoId: pessoa.Endereco.Cidade.Estado.Id,
            Pais: pessoa.Endereco.Cidade.Estado.Pais.Descricao,
            PaisId: pessoa.Endereco.Cidade.Estado.Pais.Id
        };
        this.paisSelected = pessoa.Endereco.Cidade.Estado.Pais.Id;
        this.estadoSelected = pessoa.Endereco.Cidade.Estado.Id;
        this.onChangeEstado(pessoa.Endereco.Cidade.Estado.Id);
        this.cidadeSelected = pessoa.Endereco.Cidade.Id;
    }

    onChangeEstado(selectedValue: any) {
        this.pessoa.getCidades(selectedValue)
            .subscribe(data => {
                console.log(data)
                this.cidades = data;
            });
    }

    presentLoading() {
        this.loader = this.loadingCtrl.create({ content: "Por favor aguarde...", duration: 5000 });
        this.loader.present();
    }

    ionViewDidLoad() {
        console.log('ionViewDidLoad ClientePage');
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
