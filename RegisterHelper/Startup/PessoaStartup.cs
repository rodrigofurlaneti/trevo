using System;
using Entidade;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Aplicacao;
using Entidade.Uteis;
using InitializerHelper.Startup.Cidades;

namespace InitializerHelper.Startup
{
    public static class PessoaStartup
    {
        #region Private Members
        private static void AdicionaPessoaRoot()
        {
            var pessoaAplicacao = ServiceLocator.Current.GetInstance<IPessoaAplicacao>();
            var pessoaRoot = pessoaAplicacao.PrimeiroPor(x => x.Nome.Equals("Administrador"));
            if (pessoaRoot != null)
                return;
            //país 
            var paisAplicacao = ServiceLocator.Current.GetInstance<IPaisAplicacao>();
            var pais = paisAplicacao.PrimeiroPor(x => x.Descricao.Equals("Brasil"));
            var paisList = new List<Pais>();
            if (pais == null)
            {
                #region todos pais
                paisAplicacao.Salvar(new Pais { Descricao = "Brasil" });
                paisAplicacao.Salvar(new Pais { Descricao = "Afeganistão" });
                paisAplicacao.Salvar(new Pais { Descricao = "Albânia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Argélia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Samoa Americana" });
                paisAplicacao.Salvar(new Pais { Descricao = "Andorra" });
                paisAplicacao.Salvar(new Pais { Descricao = "Angola" });
                paisAplicacao.Salvar(new Pais { Descricao = "Anguilla" });
                paisAplicacao.Salvar(new Pais { Descricao = "Antártida" });
                paisAplicacao.Salvar(new Pais { Descricao = "Antigua e Barbuda" });
                paisAplicacao.Salvar(new Pais { Descricao = "Argentina" });
                paisAplicacao.Salvar(new Pais { Descricao = "Armênia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Aruba" });
                paisAplicacao.Salvar(new Pais { Descricao = "Austrália" });
                paisAplicacao.Salvar(new Pais { Descricao = "Áustria" });
                paisAplicacao.Salvar(new Pais { Descricao = "Azerbaijão" });
                paisAplicacao.Salvar(new Pais { Descricao = "Bahamas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Bahrein" });
                paisAplicacao.Salvar(new Pais { Descricao = "Bangladesh" });
                paisAplicacao.Salvar(new Pais { Descricao = "Barbados" });
                paisAplicacao.Salvar(new Pais { Descricao = "Belarus" });
                paisAplicacao.Salvar(new Pais { Descricao = "Bélgica" });
                paisAplicacao.Salvar(new Pais { Descricao = "Belize" });
                paisAplicacao.Salvar(new Pais { Descricao = "Benin" });
                paisAplicacao.Salvar(new Pais { Descricao = "Bermudas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Butão" });
                paisAplicacao.Salvar(new Pais { Descricao = "Bolívia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Bósnia-herzegovina" });
                paisAplicacao.Salvar(new Pais { Descricao = "Botsuana" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ilha Bouvet" });
                paisAplicacao.Salvar(new Pais { Descricao = "Território Britânico do Oceano Indico" });
                paisAplicacao.Salvar(new Pais { Descricao = "Brunei" });
                paisAplicacao.Salvar(new Pais { Descricao = "Bulgária" });
                paisAplicacao.Salvar(new Pais { Descricao = "Burkina Faso" });
                paisAplicacao.Salvar(new Pais { Descricao = "Burundi" });
                paisAplicacao.Salvar(new Pais { Descricao = "Camboja" });
                paisAplicacao.Salvar(new Pais { Descricao = "Camarões" });
                paisAplicacao.Salvar(new Pais { Descricao = "Canada" });
                paisAplicacao.Salvar(new Pais { Descricao = "Cabo Verde, Republica de" });
                paisAplicacao.Salvar(new Pais { Descricao = "Cayman, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Republica Centro-Africana" });
                paisAplicacao.Salvar(new Pais { Descricao = "Chade" });
                paisAplicacao.Salvar(new Pais { Descricao = "Chile" });
                paisAplicacao.Salvar(new Pais { Descricao = "China, Republica Popular" });
                paisAplicacao.Salvar(new Pais { Descricao = "Christmas, Ilha (Navidad)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Cocos(Keeling), Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Colômbia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Comores, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Congo" });
                paisAplicacao.Salvar(new Pais { Descricao = "Congo, Republica Democrática do" });
                paisAplicacao.Salvar(new Pais { Descricao = "Cook, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Costa Rica" });
                paisAplicacao.Salvar(new Pais { Descricao = "Costa do Marfim" });
                paisAplicacao.Salvar(new Pais { Descricao = "Croácia (Republica da)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Cuba" });
                paisAplicacao.Salvar(new Pais { Descricao = "Chipre" });
                paisAplicacao.Salvar(new Pais { Descricao = "Tcheca, Republica" });
                paisAplicacao.Salvar(new Pais { Descricao = "Dinamarca" });
                paisAplicacao.Salvar(new Pais { Descricao = "Djibuti" });
                paisAplicacao.Salvar(new Pais { Descricao = "Dominica, Ilha" });
                paisAplicacao.Salvar(new Pais { Descricao = "Republica Dominicana" });
                paisAplicacao.Salvar(new Pais { Descricao = "Timor Leste" });
                paisAplicacao.Salvar(new Pais { Descricao = "Equador" });
                paisAplicacao.Salvar(new Pais { Descricao = "Egito" });
                paisAplicacao.Salvar(new Pais { Descricao = "El Salvador" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guine-Equatorial" });
                paisAplicacao.Salvar(new Pais { Descricao = "Eritreia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Estônia, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Etiópia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Falkland (Ilhas Malvinas)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Feroe, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Fiji" });
                paisAplicacao.Salvar(new Pais { Descricao = "Finlândia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Franca" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guiana francesa" });
                paisAplicacao.Salvar(new Pais { Descricao = "Polinésia Francesa" });
                paisAplicacao.Salvar(new Pais { Descricao = "Terras Austrais e Antárticas Francesas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Gabão" });
                paisAplicacao.Salvar(new Pais { Descricao = "Gambia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Georgia, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Alemanha" });
                paisAplicacao.Salvar(new Pais { Descricao = "Gana" });
                paisAplicacao.Salvar(new Pais { Descricao = "Gibraltar" });
                paisAplicacao.Salvar(new Pais { Descricao = "Grécia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Groenlândia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Granada" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guadalupe" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guam" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guatemala" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guine" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guine-Bissau" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guiana" });
                paisAplicacao.Salvar(new Pais { Descricao = "Haiti" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ilha Heard e Ilhas McDonald" });
                paisAplicacao.Salvar(new Pais { Descricao = "Vaticano, Estado da Cidade do" });
                paisAplicacao.Salvar(new Pais { Descricao = "Honduras" });
                paisAplicacao.Salvar(new Pais { Descricao = "Hong Kong" });
                paisAplicacao.Salvar(new Pais { Descricao = "Hungria, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Islândia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Índia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Indonésia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ira, Republica Islâmica do" });
                paisAplicacao.Salvar(new Pais { Descricao = "Iraque" });
                paisAplicacao.Salvar(new Pais { Descricao = "Irlanda" });
                paisAplicacao.Salvar(new Pais { Descricao = "Israel" });
                paisAplicacao.Salvar(new Pais { Descricao = "Itália" });
                paisAplicacao.Salvar(new Pais { Descricao = "Jamaica" });
                paisAplicacao.Salvar(new Pais { Descricao = "Japão" });
                paisAplicacao.Salvar(new Pais { Descricao = "Jordânia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Cazaquistão, Republica do" });
                paisAplicacao.Salvar(new Pais { Descricao = "Quênia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Kiribati" });
                paisAplicacao.Salvar(new Pais { Descricao = "Coreia, Republica Popular Democrática da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Coreia, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Coveite" });
                paisAplicacao.Salvar(new Pais { Descricao = "Quirguiz, Republica" });
                paisAplicacao.Salvar(new Pais { Descricao = "Laos, Republica Popular Democrática do" });
                paisAplicacao.Salvar(new Pais { Descricao = "Letônia, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Líbano" });
                paisAplicacao.Salvar(new Pais { Descricao = "Lesoto" });
                paisAplicacao.Salvar(new Pais { Descricao = "Libéria" });
                paisAplicacao.Salvar(new Pais { Descricao = "Líbia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Liechtenstein" });
                paisAplicacao.Salvar(new Pais { Descricao = "Lituânia, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Luxemburgo" });
                paisAplicacao.Salvar(new Pais { Descricao = "Macau" });
                paisAplicacao.Salvar(new Pais { Descricao = "Macedônia, Antiga Republica Iugoslava" });
                paisAplicacao.Salvar(new Pais { Descricao = "Madagascar" });
                paisAplicacao.Salvar(new Pais { Descricao = "Malavi" });
                paisAplicacao.Salvar(new Pais { Descricao = "Malásia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Maldivas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Mali" });
                paisAplicacao.Salvar(new Pais { Descricao = "Malta" });
                paisAplicacao.Salvar(new Pais { Descricao = "Marshall, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Martinica" });
                paisAplicacao.Salvar(new Pais { Descricao = "Mauritânia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Mauricio" });
                paisAplicacao.Salvar(new Pais { Descricao = "Mayotte (Ilhas Francesas)" });
                paisAplicacao.Salvar(new Pais { Descricao = "México" });
                paisAplicacao.Salvar(new Pais { Descricao = "Micronesia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Moldávia, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Mônaco" });
                paisAplicacao.Salvar(new Pais { Descricao = "Mongólia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Montserrat, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Marrocos" });
                paisAplicacao.Salvar(new Pais { Descricao = "Moçambique" });
                paisAplicacao.Salvar(new Pais { Descricao = "Mianmar (Birmânia)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Namíbia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Nauru" });
                paisAplicacao.Salvar(new Pais { Descricao = "Nepal" });
                paisAplicacao.Salvar(new Pais { Descricao = "Países Baixos (Holanda)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Antilhas Holandesas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Nova Caledonia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Nova Zelândia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Nicarágua" });
                paisAplicacao.Salvar(new Pais { Descricao = "Níger" });
                paisAplicacao.Salvar(new Pais { Descricao = "Nigéria" });
                paisAplicacao.Salvar(new Pais { Descricao = "Niue, Ilha" });
                paisAplicacao.Salvar(new Pais { Descricao = "Norfolk, Ilha" });
                paisAplicacao.Salvar(new Pais { Descricao = "Marianas do Norte" });
                paisAplicacao.Salvar(new Pais { Descricao = "Noruega" });
                paisAplicacao.Salvar(new Pais { Descricao = "Oma" });
                paisAplicacao.Salvar(new Pais { Descricao = "Paquistão" });
                paisAplicacao.Salvar(new Pais { Descricao = "Palau" });
                paisAplicacao.Salvar(new Pais { Descricao = "Panamá" });
                paisAplicacao.Salvar(new Pais { Descricao = "Papua Nova Guine" });
                paisAplicacao.Salvar(new Pais { Descricao = "Paraguai" });
                paisAplicacao.Salvar(new Pais { Descricao = "Peru" });
                paisAplicacao.Salvar(new Pais { Descricao = "Filipinas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Pitcairn, Ilha" });
                paisAplicacao.Salvar(new Pais { Descricao = "Polônia, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Portugal" });
                paisAplicacao.Salvar(new Pais { Descricao = "Porto Rico" });
                paisAplicacao.Salvar(new Pais { Descricao = "Catar" });
                paisAplicacao.Salvar(new Pais { Descricao = "Reunião, Ilha" });
                paisAplicacao.Salvar(new Pais { Descricao = "Romênia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Rússia, Federação da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ruanda" });
                paisAplicacao.Salvar(new Pais { Descricao = "São Cristovão e Neves, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Santa Lucia" });
                paisAplicacao.Salvar(new Pais { Descricao = "São Vicente e Granadinas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Samoa" });
                paisAplicacao.Salvar(new Pais { Descricao = "San Marino" });
                paisAplicacao.Salvar(new Pais { Descricao = "São Tome e Príncipe, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Arábia Saudita" });
                paisAplicacao.Salvar(new Pais { Descricao = "Senegal" });
                paisAplicacao.Salvar(new Pais { Descricao = "Seychelles" });
                paisAplicacao.Salvar(new Pais { Descricao = "Serra Leoa" });
                paisAplicacao.Salvar(new Pais { Descricao = "Cingapura" });
                paisAplicacao.Salvar(new Pais { Descricao = "Eslovaca, Republica" });
                paisAplicacao.Salvar(new Pais { Descricao = "Eslovênia, Republica da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Salomão, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Somalia" });
                paisAplicacao.Salvar(new Pais { Descricao = "África do Sul" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ilhas Geórgia do Sul e Sandwich do Sul" });
                paisAplicacao.Salvar(new Pais { Descricao = "Espanha" });
                paisAplicacao.Salvar(new Pais { Descricao = "Sri Lanka" });
                paisAplicacao.Salvar(new Pais { Descricao = "Santa Helena" });
                paisAplicacao.Salvar(new Pais { Descricao = "São Pedro e Miquelon" });
                paisAplicacao.Salvar(new Pais { Descricao = "Sudão" });
                paisAplicacao.Salvar(new Pais { Descricao = "Suriname" });
                paisAplicacao.Salvar(new Pais { Descricao = "Svalbard e Jan Mayen" });
                paisAplicacao.Salvar(new Pais { Descricao = "Suazilândia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Suécia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Suíça" });
                paisAplicacao.Salvar(new Pais { Descricao = "Síria, Republica Árabe da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Formosa (Taiwan)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Tadjiquistao, Republica do" });
                paisAplicacao.Salvar(new Pais { Descricao = "Tanzânia, Republica Unida da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Tailândia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Togo" });
                paisAplicacao.Salvar(new Pais { Descricao = "Toquelau, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Tonga" });
                paisAplicacao.Salvar(new Pais { Descricao = "Trinidad e Tobago" });
                paisAplicacao.Salvar(new Pais { Descricao = "Tunísia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Turquia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Turcomenistão, Republica do" });
                paisAplicacao.Salvar(new Pais { Descricao = "Turcas e Caicos, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Tuvalu" });
                paisAplicacao.Salvar(new Pais { Descricao = "Uganda" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ucrânia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Emirados Árabes Unidos" });
                paisAplicacao.Salvar(new Pais { Descricao = "Reino Unido" });
                paisAplicacao.Salvar(new Pais { Descricao = "Estados Unidos" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ilhas Menores Distantes dos Estados Unidos" });
                paisAplicacao.Salvar(new Pais { Descricao = "Uruguai" });
                paisAplicacao.Salvar(new Pais { Descricao = "Uzbequistão, Republica do" });
                paisAplicacao.Salvar(new Pais { Descricao = "Vanuatu" });
                paisAplicacao.Salvar(new Pais { Descricao = "Venezuela" });
                paisAplicacao.Salvar(new Pais { Descricao = "Vietnã" });
                paisAplicacao.Salvar(new Pais { Descricao = "Virgens, Ilhas (Britânicas)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Virgens, Ilhas (E.U.A.)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Wallis e Futuna, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Saara Ocidental" });
                paisAplicacao.Salvar(new Pais { Descricao = "Iémen" });
                paisAplicacao.Salvar(new Pais { Descricao = "Iugoslávia, República Fed. da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Zâmbia" });
                paisAplicacao.Salvar(new Pais { Descricao = "Zimbabue" });
                paisAplicacao.Salvar(new Pais { Descricao = "Guernsey, Ilha do Canal (Inclui Alderney e Sark)" });
                paisAplicacao.Salvar(new Pais { Descricao = "Jersey, Ilha do Canal" });
                paisAplicacao.Salvar(new Pais { Descricao = "Canarias, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Man, Ilha de" });
                paisAplicacao.Salvar(new Pais { Descricao = "Johnston, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Madeira, Ilha da" });
                paisAplicacao.Salvar(new Pais { Descricao = "Montenegro" });
                paisAplicacao.Salvar(new Pais { Descricao = "Republika Srbija" });
                paisAplicacao.Salvar(new Pais { Descricao = "Sudao do Sul" });
                paisAplicacao.Salvar(new Pais { Descricao = "Zona do Canal do Panamá" });
                paisAplicacao.Salvar(new Pais { Descricao = "Wake, Ilha" });
                paisAplicacao.Salvar(new Pais { Descricao = "Lebuan, Ilhas" });
                paisAplicacao.Salvar(new Pais { Descricao = "Palestina" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ilhas de Aland" });
                paisAplicacao.Salvar(new Pais { Descricao = "Coletividade de São Bartolomeu" });
                paisAplicacao.Salvar(new Pais { Descricao = "Curaçao" });
                paisAplicacao.Salvar(new Pais { Descricao = "Ilha de São Martinho (França)" });
                paisAplicacao.Salvar(new Pais { Descricao = "São Martinho (Países Baixos)" });
                #endregion
            }
            //estado
            var estadoAplicacao = ServiceLocator.Current.GetInstance<IEstadoAplicacao>();
            var estado = estadoAplicacao.PrimeiroPor(x => x.Descricao.Equals("São Paulo"));
            var estadoList = new List<Estado>();
            if (estado == null)
            {
                pais = paisAplicacao.PrimeiroPor(x => x.Descricao.Equals("Brasil"));
                #region todos estados
                estadoAplicacao.Salvar(new Estado { Descricao = "Acre", Sigla = "AC", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Alagoas", Sigla = "AL", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Amazonas", Sigla = "AM", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Amapá", Sigla = "AP", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Bahia", Sigla = "BA", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Ceará", Sigla = "CE", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Distrito Federal", Sigla = "DF", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Espírito Santo", Sigla = "ES", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Goiás", Sigla = "GO", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Maranhão", Sigla = "MA", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Minas Gerais", Sigla = "MG", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Mato Grosso do Sul", Sigla = "MS", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Mato Grosso", Sigla = "MT", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Pará", Sigla = "PA", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Paraíba", Sigla = "PB", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Pernambuco", Sigla = "PE", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Piauí", Sigla = "PI", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Paraná", Sigla = "PR", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Rio de Janeiro", Sigla = "RJ", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Rio Grande do Norte", Sigla = "RN", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Rondônia", Sigla = "RO", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Roraima", Sigla = "RR", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Rio Grande do Sul", Sigla = "RS", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Santa Catarina", Sigla = "SC", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Sergipe", Sigla = "SE", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "São Paulo", Sigla = "SP", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Tocantins", Sigla = "TO", Pais = pais });
                estadoAplicacao.Salvar(new Estado { Descricao = "Exterior", Sigla = "EX", Pais = null });
                #endregion
            }
            //cidade
            var cidadeAplicacao = ServiceLocator.Current.GetInstance<ICidadeAplicacao>();
            var cidade = cidadeAplicacao.PrimeiroPor(x => x.Descricao.Equals("São Paulo"));
            var cidadeList = new List<Cidade>();
            if (cidade == null)
            {
                Cidade1.SalvaCidade(cidadeAplicacao, estadoAplicacao);
                Cidade2.SalvaCidade(cidadeAplicacao, estadoAplicacao);
                Cidade3.SalvaCidade(cidadeAplicacao, estadoAplicacao);
                Cidade4.SalvaCidade(cidadeAplicacao, estadoAplicacao);
                Cidade5.SalvaCidade(cidadeAplicacao, estadoAplicacao);
            }
            //endereco
            cidade = cidadeAplicacao.PrimeiroPor(x => x.Descricao.Equals("São Paulo"));

            var enderecoAplicacao = ServiceLocator.Current.GetInstance<IEnderecoAplicacao>();
            var endereco = new Endereco
            {
                Cep = "03637-000",
                Bairro = "Penha",
                Logradouro = "Rua Padre João",
                Numero = "444",
                Tipo = "Rua",
                Complemento = "Cj. 125",
                Descricao = "Matriz 4WORLD",
                Latitude = "-23.5238722",
                Longitude = "-46.5468393",
                Cidade = cidade
            };
            enderecoAplicacao.Salvar(endereco);
            
            //
            var pessoa = new Pessoa
            {
                Nome = "Administrador",
                Sexo = "Masculino",
                DataNascimento = new DateTime(1988, 12, 23),
                Documentos = new List<PessoaDocumento> { new PessoaDocumento { Documento = new Documento(TipoDocumento.Cpf, "523.203.170-82", DateTime.Now) } },
                Contatos = new List<PessoaContato>
                {
                    new PessoaContato { Contato = new Contato {Numero = "1199999999", Tipo = TipoContato.Celular} },
                    new PessoaContato { Contato = new Contato { Email = "administrador@4world.com.br", Tipo = TipoContato.Email }}
                },
                Enderecos = new List<PessoaEndereco> { new PessoaEndereco{ Endereco = endereco}}
            };
            pessoaAplicacao.Salvar(pessoa);
        }
        #endregion

        #region Public Members
        public static void Start()
        {
            AdicionaPessoaRoot();
        }
        #endregion
    }
}