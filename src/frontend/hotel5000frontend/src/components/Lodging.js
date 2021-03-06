import React, {Component} from 'react';
import {
    Accordion,
    Button,
    Card,
    FormControl,
    FormLabel,
    InputGroup,
    ListGroup,
    ListGroupItem,
    Modal
} from "react-bootstrap";
import {FaSearch} from "react-icons/fa";
import {refresh} from "./RefreshHelper";
import {BaseUrl} from './FetchHelper'



class Lodging extends Component {

    constructor(props) {
        super(props);

        this.setSelectedPaymentType = this.setSelectedPaymentType.bind(this)

        this.state = {
            pageNumber: 1,
            resultPerPage: 9,
            data: [],
            nextPageData: [],
            rooms: [],
            fromDateFilter: "",
            toDateFilter: "",
            isSearchClicked: false,
            modalIndex: null,
            freeIntervals: [],
            countryFilter: "",

            paymentType: "Cash",
            reservedFrom: null,
            reservedTo: null,
            reservationItems: []
        }
        this.reservationItem = {
            "reservedFrom": "",
            "reservedTo": "",
            "roomId": null
        }
    }

    componentDidMount() {
        this.getCurrentLodgings()
        this.getNextData()
    }

    componentDidUpdate(prevProps, prevState) {
        if(prevState.pageNumber !== this.state.pageNumber ||
            prevState.isSearchClicked !== this.state.isSearchClicked ||
            (prevState.countryFilter !== "" && this.state.countryFilter === "") ||
            (prevState.fromDateFilter!== "" && this.state.fromDateFilter === "") ||
            (prevState.toDateFilter !== "" && this.state.toDateFilter === "")){
            this.getCurrentLodgings();
            this.getNextData();
        }
        console.log(this.state.reservationItems)
    }

    increasePageNumber = () => {
        if(this.state.nextPageData.length !== 0)
            this.setState({pageNumber: this.state.pageNumber + 1})
    }

    decreasePageNumber = () => {
        if(this.state.pageNumber > 1)
            this.setState({pageNumber: this.state.pageNumber - 1})
    }

    getCurrentLodgings = () => {
        let url = new URL(BaseUrl + "api/lodgings"),
            params = {pageNumber: this.state.pageNumber, resultPerPage: this.state.resultPerPage,
                      reservableFrom: this.state.fromDateFilter, reservableTo: this.state.toDateFilter,
                      address: this.state.countryFilter}
            Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

        fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {

            }
        })
            .then(resp => resp.json())
            .then(responseJson => {
                console.log(responseJson)
                this.setState({data: responseJson})
            })
    }

    getNextData = () => {
        let url = new URL(BaseUrl + "api/lodgings"),
            params = {pageNumber: this.state.pageNumber + 1, resultPerPage: this.state.resultPerPage,
                      reservableFrom: this.state.fromDateFilter, reservableTo: this.state.toDateFilter,
                      address: this.state.countryFilter}
        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

        fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {

            }
        })
            .then(resp => resp.json())
            .then(responseJson => {
                console.log(responseJson)
                this.setState({nextPageData: responseJson})
            })
    }

    handleOpenModal(e, id){
        this.setState({modalIndex: id});
    }

    handleCloseModal(){
        this.setState({modalIndex: null})
    }

    formatDate = (dateString) => {
        let options = {year: 'numeric', month: 'numeric', day: 'numeric'}
        return new Date(dateString).toLocaleDateString([], options)
    }

    getFreeIntervals = (roomId) => {
        fetch(BaseUrl + "api/reservations/rooms/" + roomId, {
            method: 'GET',
            mode: "cors",
            headers: {

            }
        })
            .then(response => response.json())
            .then(responseJson => {
                this.setState({freeIntervals: responseJson})
            })
    }

    //region Reservation
    setSelectedPaymentType(event) {
        this.setState({paymentType: event.target.value})
    }
    handleReservedFromChanged = (from) => {
        this.reservationItem.reservedFrom = from
    }
    handleReservedToChanged  = (to) => {
        this.reservationItem.reservedTo = to
    }
    addReservationItem = (item) => {
        const currentItems = this.state.reservationItems
        const newItems = currentItems.concat(JSON.parse(JSON.stringify(item)))
        this.setState({reservationItems: newItems})
        alert("Item added to reservation")
    }
    reserveRoom = (paymentType, reservationItems) => {
        const data = {
            "email": sessionStorage.getItem('email'),
            "paymentType": paymentType,
            "reservationItems": reservationItems
        }

        fetch(BaseUrl + "api/reservations", {
            method: 'POST',
            mode: "cors",
            headers: {
                "Content-Type":"application/json",
                "Authorization":"Bearer " + sessionStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if(response.status === 200){
                    alert("successful reservation")
                    window.location.reload(false)
                }
                else if(response.status === 401){
                    let token = response.headers.get('token-expired')
                    if(token) {
                        refresh().then(() => {
                            this.reserveRoom(paymentType, reservationItems)
                        }).catch((error) => {
                            console.log(error)
                        })
                    }
                }
            })
    }
    //endregion

    renderLodgings = () => {

        const rows = [...Array(Math.ceil(this.state.data.length / 3))]

        const lodgingRows = rows.map((row, index) => this.state.data.slice(index * 3, index * 3 + 3))

        const content = lodgingRows.map((row, index) => {
            return(
                <div className="row" key={index}>
                    {row.map(lodging => {
                        return (
                            <div className="mx-auto my-3" key={lodging.id}>
                                <Card style={{width: '25rem'}}>
                                    <Card.Body>
                                        <Card.Title>{lodging.name}</Card.Title>
                                        <Card.Subtitle>{lodging.lodgingType}</Card.Subtitle>
                                    </Card.Body>
                                    <ListGroup className="list-group-flush">
                                        <ListGroupItem>
                                            <Button variant="outline-dark" onClick={e => this.handleOpenModal(e, lodging.id)}>
                                                Details
                                            </Button>
                                            <Modal show={this.state.modalIndex === lodging.id} onHide={() => this.handleCloseModal()}>
                                                <Modal.Header closeButton>
                                                    <Modal.Title>{lodging.name}</Modal.Title>
                                                </Modal.Header>
                                                <Modal.Body>
                                                    <h5 className="mt-3">Rooms</h5>
                                                    {lodging.rooms.map( room => {
                                                        return(
                                                            <div key={room.id}>
                                                                <Accordion>
                                                                    <Card>
                                                                        <Card.Header>
                                                                            <Accordion.Toggle as={Button} variant="outline-dark"
                                                                                              eventKey={room.id} onClick={() => this.getFreeIntervals(room.id)}>
                                                                                {room.adultCapacity + room.childrenCapacity} capacity room
                                                                            </Accordion.Toggle>
                                                                        </Card.Header>
                                                                        <Accordion.Collapse eventKey={room.id}>
                                                                            <ListGroup>
                                                                                <ListGroupItem>
                                                                                    Adult capacity: {room.adultCapacity}
                                                                                </ListGroupItem>
                                                                                <ListGroupItem>
                                                                                    Children capacity: {room.childrenCapacity}
                                                                                </ListGroupItem>
                                                                                <ListGroupItem>
                                                                                    Price: {room.price} {room.currency}
                                                                                </ListGroupItem>
                                                                                <ListGroupItem>
                                                                                    <h5>Free intervals</h5>
                                                                                    {Array.from(this.state.freeIntervals).map((interval, index) => {
                                                                                        return(
                                                                                            <div key={index}>
                                                                                                <FormLabel>From</FormLabel>
                                                                                                <FormLabel className="mx-2">{this.formatDate(interval.from)}</FormLabel>
                                                                                                <FormLabel>To</FormLabel>
                                                                                                <FormLabel className="mx-2">{this.formatDate(interval.to)}</FormLabel>
                                                                                            </div>
                                                                                        )
                                                                                    })}
                                                                                </ListGroupItem>
                                                                                <ListGroupItem>
                                                                                    <FormLabel>Payment type</FormLabel>
                                                                                    <FormControl as="select">
                                                                                        <option value="Cash">Cash</option>
                                                                                        <option value="Card">Card</option>
                                                                                    </FormControl>
                                                                                </ListGroupItem>
                                                                                <ListGroupItem>
                                                                                    <FormLabel className="mr-3">Date of arrival</FormLabel>
                                                                                    <input type="date"
                                                                                           onChange={(from) => this.handleReservedFromChanged(from.target.value)}/><br/>
                                                                                    <FormLabel className="mr-3">Date of leave</FormLabel>
                                                                                    <input type="date"
                                                                                           onChange={(to) => this.handleReservedToChanged(to.target.value)}/>
                                                                                </ListGroupItem>
                                                                                <Button variant="outline-dark"
                                                                                        onClick={() => {this.reservationItem.roomId = room.id;
                                                                                            this.addReservationItem(this.reservationItem);}}>
                                                                                    Add item to reservation
                                                                                </Button>
                                                                            </ListGroup>
                                                                        </Accordion.Collapse>
                                                                    </Card>
                                                                </Accordion>
                                                            </div>
                                                        )}
                                                    )}
                                                    <Button variant="success" className="mx-auto"
                                                            hidden={this.reservationItem.roomId === null}
                                                            onClick={() => this.reserveRoom(this.state.paymentType, this.state.reservationItems)}>
                                                        Send reservation
                                                    </Button>
                                                    <h5 className="mt-3">Address</h5>
                                                    {lodging.lodgingAddresses.map( address => {
                                                        return(
                                                          <div key={address.id}>
                                                              <ListGroup>
                                                                  <ListGroupItem>
                                                                      {address.countryCode}, {address.country}, {address.county}, {address.postalCode},
                                                                      {address.city}, {address.street}, {address.houseNumber}
                                                                  </ListGroupItem>
                                                                  <ListGroupItem hidden={address.floor === null}>
                                                                      Floor: {address.floor}
                                                                  </ListGroupItem>
                                                                  <ListGroupItem hidden={address.doorNumber === null}>
                                                                      Door number: {address.doorNumber}
                                                                  </ListGroupItem>
                                                              </ListGroup>
                                                          </div>
                                                        )}
                                                    )}
                                                    <h5 className="mt-3">Free</h5>
                                                    {lodging.reservationWindows.map( resWindow => {
                                                        return(
                                                          <div key={resWindow.id} className="mx-auto" style={{textAlign: 'center'}}>
                                                              <p>From: {this.formatDate(resWindow.from)} &emsp; To: {this.formatDate(resWindow.to)}</p>
                                                          </div>
                                                        )}
                                                    )}
                                                </Modal.Body>
                                            </Modal>

                                        </ListGroupItem>
                                    </ListGroup>
                                </Card>
                            </div>


                        )
                    })}
                </div>
            )
        })

        return(
            <div>
                {content}
            </div>
        )
    }

    //region Searchbar handlers
    handleFromDateChanged = (from) => {
        this.setState({fromDateFilter: from})
        console.log(from)
    }

    handleCountryChanged = (country) => {
        this.setState({countryFilter: country.target.value})
    }

    handleToDateChanged = (to) => {
        this.setState({toDateFilter: to})
        console.log(to)
    }

    handleSearchClick = () => {
        this.setState({isSearchClicked: !this.state.isSearchClicked, pageNumber: 1})
    }
    //endregion

    render() {
        return (
            <div>
                <div style={{width: '50rem'}} className="mx-auto mt-3">
                    <InputGroup id="input">
                        <FormControl size="lg" placeholder="Country of holiday" onChange={(country) => {this.handleCountryChanged(country)}}/>

                        <input type="date" as={InputGroup.Append} onChange={(event) => this.handleFromDateChanged(event.target.value)}/>

                        <input type="date" as={InputGroup.Append} onChange={(event) => this.handleToDateChanged(event.target.value)}/>
                        <Button variant="outline-dark" as={InputGroup.Append} onClick={() => this.handleSearchClick()}><FaSearch/>Search</Button>
                    </InputGroup>
                </div>
                {this.renderLodgings()}
                <div className="buttonContainer mx-auto my-3">
                    <Button variant="outline-dark" onClick={() => this.decreasePageNumber()} className="pageButton mr-2" disabled={this.state.pageNumber === 1}>Previous page</Button>
                    <Button variant="outline-dark" onClick={() => this.increasePageNumber()} className="pageButton ml-2" disabled={this.state.nextPageData.length === 0}>Next page</Button>
                </div>
            </div>
        );
    }
}

export default Lodging;