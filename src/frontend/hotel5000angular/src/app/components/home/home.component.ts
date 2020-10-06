import { Component, OnInit } from '@angular/core';
import { Lodging } from 'src/app/models/Lodging';
import { LodgingService } from 'src/app/services/lodging.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  lodgings: Lodging[];

  constructor(private lodgingService: LodgingService) { }

  ngOnInit(): void {
    this.lodgingService.getLodgings().subscribe(lodgings => {
      this.lodgings = lodgings.result;
    });
  }

}