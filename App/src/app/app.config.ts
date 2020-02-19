//import { OpaqueToken } from '@angular/core';

//// Although the ApplicationConfig interface plays no role in dependency injection,
//// it supports typing of the configuration object within the class.
//export interface IApplicationConfig {
//    apiEndpoint: string;
//}

//// Configuration values for our app
//export const Configuration: IApplicationConfig = {
//    apiEndpoint: 'http://localhost:59662/'
//};

//// Create a config token to avoid naming conflicts
//export const ConfigurationToken = new OpaqueToken('config');

import { Injectable } from '@angular/core';

@Injectable()
export class ConfigurationSettings {
    //ApiUrl = //"http://localhost:59662/"
    //public static apiUrl : string = "http://localhost:59662/";
    public static apiUrl: string = "http://192.168.0.13:59662/";
    //public static apiUrl: string = "http://apidrivethru.4world.com.br/";
}