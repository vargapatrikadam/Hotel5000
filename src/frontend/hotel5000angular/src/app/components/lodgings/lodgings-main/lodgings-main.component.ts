import { Component, Input, OnInit } from '@angular/core';
import { Lodging } from 'src/app/models/Lodging';
import { LodgingService } from 'src/app/services/lodging.service';

@Component({
  selector: 'app-lodgings-main',
  templateUrl: './lodgings-main.component.html',
  styleUrls: ['./lodgings-main.component.css']
})
export class LodgingsMainComponent implements OnInit {
  lodgings: Lodging[];

  constructor(private lodgingService: LodgingService) { }

  ngOnInit(): void {
    this.lodgingService.getLodgings().subscribe(lodgings => {
      this.lodgings = lodgings.result;
    })
  }

}
