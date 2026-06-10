import { NgFor, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'json-to-list',
  standalone: true,
  imports: [NgIf, NgFor],
  templateUrl: './json-to-list.component.html',
  styleUrl: './json-to-list.component.css'
})
export class JsonToListComponent {
  _entries: { key: string; value: any }[] = [];

  @Input() set value(jsonData: any) {
    if (!jsonData || typeof jsonData !== 'object')
      return;

    if (jsonData instanceof Array) {
      this._entries = Array.from(jsonData).map((i) => { return { key: i.key, value: i.value } });
      return;
    }
    this._entries = Object.entries(jsonData).map(([key, value]) => ({ key, value }));
  }
}
