import { Component, Input, OnInit } from '@angular/core';
import { Lodging } from 'src/app/models/Lodging';
import { Router } from '@angular/router';

@Component({
  selector: 'app-lodgings-item',
  templateUrl: './lodgings-item.component.html',
  styleUrls: ['./lodgings-item.component.css']
})
export class LodgingsItemComponent implements OnInit {
  @Input() lodging: Lodging;

  constructor() { }

  ngOnInit(): void {
  }

}
