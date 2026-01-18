import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  private _storage: Storage;

  constructor() {
    this._storage = localStorage;
  }

  save(key: string, value: any) {
    this._storage.setItem(key, JSON.stringify(value));
  }

  retrieve(key: string): any {
    return JSON.parse(this._storage.getItem(key)!);
  }

  delete(key: string) {
    this._storage.removeItem(key);
  }

  clear() {
    this._storage.clear();
  }
}
