import { ReservationWindow } from './ReservationWindow';

export class Room{
    id: number;
    lodgingId: number;
    adultCapacity: number;
    childrenCapacity: number;
    price: number;
    currency: string;
    reservationWindows: ReservationWindow[];
}