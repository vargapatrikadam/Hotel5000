import React, {Component} from 'react';
import {refresh} from "./RefreshHelper";
import {Button, Form} from "react-bootstrap";
import {BaseUrl} from './FetchHelper'


class LodgingForm extends Component {
    constructor(props) {
        super(props);

        this.setSelectedType = this.setSelectedType.bind(this)
        this.setSelectedCurrency = this.setSelectedCurrency.bind(this)
        this.setSelectedCountry = this.setSelectedCountry.bind(this)

        this.state = {
            lodgingName: "",
            lodgingType: "Private",
            rooms: [],
            lodgingAddresses: [],
            roomsFormVisible: false,
            addressesFormVisible: false
        }

        this.room = {
            adultCapacity: null,
            childrenCapacity: null,
            price: null,
            currency: "Forint"
        }
        this.address = {
            countryCode: "HU",
            county: "",
            city: "",
            postalCode : null,
            street: "",
            houseNumber: "",
            floor: "",
            doorNumber: ""
        }
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        console.log(this.state.rooms)
        //console.log(this.state.lodgingAddresses)
    }

    //region Lodging handlers
    handleNameChanged = (name) => {
        this.setState({lodgingName: name.target.value})
    }
    setSelectedType(event){
        this.setState({lodgingType: event.target.value})
    }
    //endregion

    //region Room handlers
    setSelectedCurrency(event) {
        this.room.currency = event.target.value
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
    //endregion

    //region AddressHandlers
    setSelectedCountry(event){
        this.address.countryCode = event.target.value
    }
    handleCountyChanged = (county) => {
        this.address.county = county.target.value
    }
    handleCityChanged = (city) => {
        this.address.city = city.target.value
    }
    handlePostalCodeChanged = event => {
        this.address.postalCode = parseInt(event.target.value)
    }
    handleStreetChanged = (street) => {
        this.address.street = street.target.value
    }
    handleHouseNumberChanged = (hNumber) => {
        this.address.houseNumber = hNumber.target.value
    }
    handleFloorChanged = (floor) => {
        this.address.floor = floor.target.value
    }
    handleDoorNumberChanged = (dNumber) => {
        this.address.doorNumber = dNumber.target.value
    }
    //endregion

    addAddress = (address) => {
        const currentAddresses = this.state.lodgingAddresses
        console.log(typeof currentAddresses)
        const newAddresses = currentAddresses.concat(JSON.parse(JSON.stringify(address)))
        this.setState({lodgingAddresses: newAddresses})
        alert("Address successfully added to lodging.")
    }

    addRoom = (room) => {
        const currentRooms = this.state.rooms
        console.log(typeof currentRooms)
        const newRooms = currentRooms.concat(JSON.parse(JSON.stringify(room)))
        this.setState({rooms: newRooms})
        alert("Room successfully added to lodging.")
    }

    handleRoomsVisibilityChanged = () => {
        this.setState({roomsFormVisible: true})
    }
    handleAddressesVisibilityChanged = () => {
        this.setState({addressesFormVisible: true})
    }

    addNewLodging = (name, lodgingType, rooms, lodgingAddresses) => {
        const data = {
            "name": name,
            "lodgingType": lodgingType,
            "rooms": rooms,
            "lodgingAddresses": lodgingAddresses
        }

        console.log(data)
        fetch(BaseUrl + "api/lodgings", {
            method: 'POST',
            mode: "cors",
            headers: {
                "Content-Type":"application/json",
                "Authorization":"Bearer " + localStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if(response.status === 200){
                    alert("Successful post")
                    window.location.reload(false)
                }
                else if(response.status === 401) {
                    let token = response.headers.get('token-expired');
                    if(token) {
                        refresh().then(() => {
                            this.addNewLodging(name, lodgingType, rooms, lodgingAddresses)
                        })
                            .catch((error) => {
                                console.log(error)
                            })
                    }
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

                        <Button variant="outline-dark" hidden={this.state.roomsFormVisible}
                                onClick={() => {this.handleRoomsVisibilityChanged()}} className="mt-3">
                            Add rooms
                        </Button>

                        <div hidden={!this.state.roomsFormVisible}>
                            <Form.Group>
                                <Form.Label>Adults</Form.Label>
                                <Form.Control type="number" placeholder="Adult capacity" min="0" onChange={this.handleAdultCapacityChanged}/>
                                <Form.Label>Children</Form.Label>
                                <Form.Control type="number" placeholder="Children capacity" min="0" onChange={this.handleChildrenCapacityChanged}/>
                                <Form.Label>Price</Form.Label>
                                <Form.Control placeholder="Price of room" step="50" min="0" type="number" onChange={this.handlePriceChanged}/>
                                <Form.Label>Currency</Form.Label>
                                <Form.Control as="select" onChange={this.setSelectedCurrency}>
                                    <option value="Forint">Forint</option>
                                    <option value="Euro">Euro</option>
                                </Form.Control>
                            </Form.Group>

                            <Button variant="outline-dark" onClick={() => {console.log(this.room);this.addRoom(this.room)}}>
                                Add room to lodging
                            </Button>
                        </div>

                        <Button variant="outline-dark" hidden={this.state.addressesFormVisible}
                                onClick={() => {this.handleAddressesVisibilityChanged()}} className="mt-3">
                            Add addresses
                        </Button>

                        <div hidden={!this.state.addressesFormVisible}>
                            <Form.Group>
                                <Form.Label>Country</Form.Label>
                                <Form.Control as="select" onChange={this.setSelectedCountry}>
                                    <option value="HU">Hungary</option>
                                    <option value="SK">Slovakia</option>
                                    <option value="HR">Croatia</option>
                                    <option value="RS">Serbia</option>
                                    <option value="IT">Italy</option>
                                </Form.Control>
                                <Form.Label>County</Form.Label>
                                <Form.Control type="text" placeholder="County" onChange={(county) => this.handleCountyChanged(county)}/>
                                <Form.Label>City</Form.Label>
                                <Form.Control type="text" placeholder="City" onChange={(city) => this.handleCityChanged(city)}/>
                                <Form.Label>Postal code</Form.Label>
                                <Form.Control type="number" placeholder="Postal code" min="0" onChange={this.handlePostalCodeChanged}/>
                                <Form.Label>Street</Form.Label>
                                <Form.Control type="text" placeholder="Street" onChange={(street) => this.handleStreetChanged(street)}/>
                                <Form.Label>House number</Form.Label>
                                <Form.Control type="text" placeholder="House number" onChange={(hNumber) => this.handleHouseNumberChanged(hNumber)}/>
                                <Form.Label>Floor</Form.Label>
                                <Form.Control type="text" placeholder="Floor (optional)" onChange={(floor) => this.handleFloorChanged(floor)}/>
                                <Form.Label>Door number</Form.Label>
                                <Form.Control type="text" placeholder="Door number (optional)" onChange={(dNumber) => this.handleDoorNumberChanged(dNumber)}/>
                            </Form.Group>

                            <Button variant="outline-dark" onClick={() => {console.log(this.address); this.addAddress(this.address)}}>
                                Add address to lodging
                            </Button>
                        </div>

                    </Form>
                </div>
                <Button className="mt-2" variant="success" onClick={
                    () => this.addNewLodging(this.state.lodgingName, this.state.lodgingType,
                        Array.from(this.state.rooms), Array.from(this.state.lodgingAddresses))}>
                    Post lodging
                </Button>
            </div>
        );
    }
}

export default LodgingForm;