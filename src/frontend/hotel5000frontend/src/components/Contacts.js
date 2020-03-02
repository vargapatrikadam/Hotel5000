import React, {Component} from 'react';
import {Button} from "react-bootstrap";
import {refresh} from "./RefreshHelper";

class Contacts extends Component {

    constructor(props) {
        super(props);
        this.state={
            id: null,
            clickedId: null,
            modifiedNumber: null,
            contacts: []
        }
        this._isMounted = false;
    }


    componentDidMount() {
        this._isMounted = true
        this._isMounted && this.getContacts(this.props.id)
        this.setState({id: this.props.id})
    }

    componentWillUnmount() {
        this._isMounted = false
    }

    handleChange = (number) => {
        this.setState({modifiedNumber: number.target.value});
    }

    getContacts = (id) => {
        fetch("https://localhost:5000/api/users/" + id + "/contacts", {
            method: 'GET',
            mode: 'cors',
            headers: {

            }
        })
            .then(response => response.json())
            .then(responsejson => {
                this._isMounted && this.setState({contacts: responsejson})
            })
    }

    deleteContacts = (userId, contactId) =>{
        fetch("https://localhost:5000/api/users/" + userId + "/contacts/" + contactId, {
            method: 'DELETE',
            mode: 'cors',
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('accessToken')
            }
        })
            .then(function (response) {
                if (response.status === 401) {
                    let token = response.headers.get('token-expired');
                    if(token) {
                        console.log(token);
                        refresh()
                        if(localStorage.getItem('loggedin') === "true") {
                            this.deleteContacts(userId, contactId)
                        }
                    }
                }
                else if(response.status === 200){
                    console.log("token not expired")
                    window.location.reload(false)
                }
            })
    }

    modifyContacts = (contactId, mobileNumber) => {

        const data = {
            "id": contactId,
            "mobileNumber": mobileNumber
        }


        fetch("https://localhost:5000/api/users/contacts/" + contactId, {
            method: 'PUT',
            mode: "cors",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(function (response) {
                if(response.status === 200){
                    this.state.contacts.map(contact => {
                        if(contact.id === contactId)
                            contact.mobileNumber = this.state.modifiedNumber
                        return contact.mobileNumber
                    })
                }
                if(response.status === 401){
                    let token = response.headers.get('token-expired');
                    if(token) {
                        console.log(token);
                        refresh()
                        if(localStorage.getItem('loggedin') === "true") {
                            this.modifyContacts(contactId, mobileNumber)
                        }
                    }
                }
                else if(response.status === 200){
                    console.log("token not expired")
                    window.location.reload(false)
                }


            })

    }

    renderContacts = () => {
        return this.state.contacts.map(contact => {
            return (
                <div key={contact.id}>
                    <p>{contact.mobileNumber}</p>
                    <div>
                        <input type="text" hidden={this.state.clickedId !== contact.id} className="mb-3 mr-1" style={{display: 'inline-block'}} onChange={(number) => this.handleChange(number)}/>
                        <Button variant="outline-dark" hidden={this.state.clickedId !== contact.id} onClick={() => this.modifyContacts(contact.id, this.state.modifiedNumber)}>Perform</Button>
                    </div>
                    <Button variant="outline-dark" className="mr-3" onClick={() => this.setState({clickedId: contact.id})}>Modify</Button>
                    <Button variant="danger" onClick={() => { if (window.confirm('Are you sure you wish to delete this contact?')) this.deleteContacts(this.state.id, contact.id)}}>Delete</Button>
                </div>
            )
        })
    }

    render() {
        return (
            <div>
                {this.renderContacts()}
            </div>
        );
    }
}

export default Contacts;