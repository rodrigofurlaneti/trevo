import { Injectable } from '@angular/core';
import { Storage } from '@ionic/storage';

import 'rxjs/add/operator/map';

@Injectable()
export class StorageCtrl {

    constructor(public storage: Storage) { }

    public claims : any;

    set(key, value) {
        return this.storage.ready().then(() => {
            return this.storage.set(key, value);
        });
    }

    get(key): Promise<any> {
        return this.storage.ready().then(() => {
            return this.storage.get(key);
        });
    }

    clear(): Promise<any> {
        return this.storage.ready().then(() => {
            return this.storage.clear();
        });
    }
}