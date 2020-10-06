import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LodgingsItemComponent } from './lodgings-item.component';

describe('LodgingsItemComponent', () => {
  let component: LodgingsItemComponent;
  let fixture: ComponentFixture<LodgingsItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LodgingsItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LodgingsItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
