import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  constructor() { }

  isNavbarCollapsed: Boolean;

  ngOnInit(): void {
    this.isNavbarCollapsed = false;
  }

  toggleNavbarCollapse() {
    this.isNavbarCollapsed = !this.isNavbarCollapsed;
    let classes = {
      'collapse': this.isNavbarCollapsed,
      'navbar-collapse': true
    }
    return classes;
  }
}
