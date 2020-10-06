import { LodgingAddress } from './LodgingAddress';
import { ReservationWindow } from './ReservationWindow';
import { Room } from './Room';

export class Lodging{
    id: number;
    name: string;
    lodgingType: string;
    userId: number;
    addresses: LodgingAddress[];
    rooms: Room[];
    reservationWindows: ReservationWindow[];
}