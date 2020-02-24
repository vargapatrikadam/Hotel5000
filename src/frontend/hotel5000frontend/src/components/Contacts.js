import React, {Component} from 'react';
import {Button} from "react-bootstrap";

class Contacts extends Component {

    constructor(props) {
        super(props);
        this.state={
            id: null,
            contacts: []
        }
    }

    componentDidMount() {
        this.handleContacts(this.props.id)
        this.setState({id: this.props.id})
    }

    handleContacts = (id) => {
        fetch("https://localhost:5000/api/users/" + id + "/contacts", {
            method: 'GET',
            mode: 'cors',
            headers: {

            }
        })
            .then(response => response.json())
            .then(responsejson => {
                this.setState({contacts: responsejson})
                console.log(this.state.contacts)
            })
    }

    renderContacts = () => {
        return this.state.contacts.map(contact => {
            return (
                <div key={contact.id}>
                    <p>{contact.mobileNumber}</p>
                    <Button variant="outline-dark" className="mr-3">Modify</Button>
                    <Button variant="danger">Delete</Button>
                </div>
            )
        })
    }
    /*TODO:confirmation required on delete button*/

    render() {
        return (
            <div>
                {this.renderContacts()}
            </div>
        );
    }
}

export default Contacts;