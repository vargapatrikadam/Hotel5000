import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Lodging } from 'src/app/models/Lodging';
import { LodgingService } from 'src/app/services/lodging.service';

@Component({
  selector: 'app-lodgings-details',
  templateUrl: './lodgings-details.component.html',
  styleUrls: ['./lodgings-details.component.css']
})
export class LodgingsDetailsComponent implements OnInit {
  lodging: Lodging;

  constructor(
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.lodging = this.route.snapshot.data.lodgingResult.result[0];
  }

}
