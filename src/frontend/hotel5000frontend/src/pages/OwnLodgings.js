import React, {Component} from 'react';
import NavBar from "../components/NavBar";
import OwnLodging from "../components/OwnLodging";

class OwnLodgings extends Component {
    render() {
        return (
            <div>
                <NavBar />
                <OwnLodging />
            </div>
        );
    }
}

export default OwnLodgings;