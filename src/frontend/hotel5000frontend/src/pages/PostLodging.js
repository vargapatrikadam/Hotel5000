import React, {Component} from 'react';
import NavBar from "../components/NavBar";
import LodgingForm from "../components/LodgingForm";

class PostLodging extends Component {
    render() {
        return (
            <div>
                <NavBar />
                <LodgingForm />
            </div>
        );
    }
}

export default PostLodging;