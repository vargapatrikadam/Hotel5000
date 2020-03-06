import React, {Component} from 'react';
import {refresh} from "./RefreshHelper";
import {Button, Form} from "react-bootstrap";

class LodgingForm extends Component {
    constructor(props) {
        super(props);

        this.state = {
            lodgingName: "",
            lodgingType: "Private",
            rooms: [],
            lodgingAddresses: []
        }

        this.room = {
            adultCapacity: null,
            childrenCapacity: null,
            price: null,
            currency: ""
        }
    }

    handleNameChanged = (name) => {
        this.setState({lodgingName: name.target.value})
    }

    handleCurrencyChanged = (currency) => {
        this.room.currency = currency.target.value
    }

    setSelectedType(event){
        this.setState({lodgingType: event.target.value})
    }

    handleAdultCapacityChanged = event =>{
        this.room.adultCapacity = parseInt(event.target.value)
    }

    handleChildrenCapacityChanged = event => {
        this.room.childrenCapacity = parseInt(event.target.value)
    }

    handlePriceChanged = event => {
        this.room.price = parseInt(event.target.value)
    }

    /*addRoom = (adultCapacity, childrenCapacity, price, currency) => {
        let obj = {
            "adultCapacity": adultCapacity,
            "childrenCapacity": childrenCapacity,
            "price": price,
            "currency": currency
        }
    }*/

    addNewLodging = (name, lodgingType, rooms, lodgingAddresses) => {
        const data = {
            "name": name,
            "lodgingType": lodgingType,
            "rooms": rooms,
            "lodgingAddresses": lodgingAddresses
        }

        console.log(data)
        fetch("https://localhost:5000/api/lodgings", {
            method: 'POST',
            mode: "cors",
            headers: {
                "Content-Type":"application/json",
                "Authorization":"Bearer " + localStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(resp => resp.status)
            .then(responseStatus => {
                if(responseStatus === 200){
                    alert("Lodging successfully posted")
                    window.location.reload()
                }
                if(responseStatus === 401){
                    refresh()
                }
            })
    }

    render() {
        return (
            <div style={{textAlign: 'center'}}>
                <div style={{width: '30rem'}} className="mx-auto mt-5">
                    <Form>
                        <Form.Group>
                            <Form.Label>Name of lodging</Form.Label>
                            <Form.Control type="text" placeholder="Name" onChange={(name) => {this.handleNameChanged(name)}}/>
                        </Form.Group>

                        <Form.Group>
                            <Form.Label>Type of lodging</Form.Label>

                            <Form.Control as="select" onChange={this.setSelectedType}>
                                <option value="Private">Post as individual</option>
                                <option value="Company">Posted as company</option>
                            </Form.Control>
                        </Form.Group>
                        {}
                        <Form.Group>
                            <Form.Label>Adults</Form.Label>
                            <Form.Control type="number" placeholder="Adult capacity" min="0" onChange={this.handleAdultCapacityChanged}/>
                            <Form.Label>Children</Form.Label>
                            <Form.Control type="number" placeholder="Children capacity" min="0" onChange={this.handleChildrenCapacityChanged}/>
                            <Form.Label>Price</Form.Label>
                            <Form.Control placeholder="Price of room" step="50" min="0" type="number" onChange={this.handlePriceChanged}/>
                            <Form.Label>Currency</Form.Label>
                            <Form.Control type="text" placeholder="Currency" onChange={(currency) => {this.handleCurrencyChanged(currency)}}/>
                        </Form.Group>

                        <Button style={{display: 'inline-block'}} variant="outline-dark" onClick={() => console.log(this.room)}>
                            Add room
                        </Button>
                    </Form>
                </div>
                <Button className="mt-2" variant="outline-dark" onClick={
                    () => this.addNewLodging(this.state.lodgingName, this.state.lodgingType,
                        this.state.rooms, this.state.lodgingAddresses)}>
                    Post lodging
                </Button>
            </div>
        );
    }
}

export default LodgingForm;