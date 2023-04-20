import { Injectable } from '@angular/core';

@Injectable()
export class LocalStorageService {

    set(key: string, value: string | undefined | null) {
        localStorage.setItem(key, (typeof value == 'string') ? value : '');
    }

    setNum(key: string, value: number) {
        localStorage.setItem(key, String(value));
    }

    get(key: string) {
        return localStorage.getItem(key);
    }

    getNum(key: string, def: number = 0) {
        const strVal = localStorage.getItem(key);
        let val: number = parseInt(strVal != null ? strVal : '0', 10);
        return isNaN(val) ? def : val;
    }

    remove(key: string) {
        localStorage.removeItem(key);
    }
}