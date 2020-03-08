import React, {Component} from 'react';
import NavBar from "../components/NavBar";
import OwnReservations from "../components/OwnReservations";

class OwnReservationsPage extends Component {
    render() {
        return (
            <div>
                <NavBar/>
                <OwnReservations/>
            </div>
        );
    }
}

export default OwnReservationsPage;