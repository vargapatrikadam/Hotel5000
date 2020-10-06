import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Lodging } from 'src/app/models/Lodging';
import { LodgingService } from 'src/app/services/lodging.service';

@Injectable({ providedIn: 'root' })
export class LodgingResolver implements Resolve<Lodging> {
  constructor(private service: LodgingService) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any>|Promise<any>|any {
    return this.service.getLodgings(parseInt(route.paramMap.get('id')));
  }
}