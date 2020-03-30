import React, {Component} from 'react';
import {Card, ListGroup, ListGroupItem} from "react-bootstrap";
import {BaseUrl} from './FetchHelper'


class OwnReservations extends Component {
    constructor(props) {
        super(props);

        this.state={
            reservations: []
        }
    }

    componentDidMount() {
        this.getOwnReservations()
    }

    getOwnReservations = () =>{
        let url = new URL(BaseUrl + "api/reservations"),
            params = {email: localStorage.getItem('email')}
            Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

        fetch(url, {
            method: 'GET',
            mode: "cors",
            headers: {}
        })
            .then(resp => resp.json())
            .then(responseJson => {
                this.setState({reservations: responseJson})
            })
    }
    formatDate = (dateString) => {
        let options = {year: 'numeric', month: 'numeric', day: 'numeric'}
        return new Date(dateString).toLocaleDateString([], options)
    }

    renderReservations = () => {
        const content = this.state.reservations.map((reservation, index) => {
            return (
                <div key={reservation.id}>
                    {reservation.reservationItems.map(item => {
                        return(
                            <div className="mx-auto my-3" key={reservation.id}>
                                <Card style={{width: '30rem'}} className="mx-auto">
                                    <Card.Body>
                                        <Card.Title>{item.lodgingName}</Card.Title>
                                    </Card.Body>
                                    <ListGroup className="list-group-flush">
                                        <ListGroupItem>
                                            Reserved from: {this.formatDate(item.reservedFrom)} to: {this.formatDate(item.reservedTo)}
                                        </ListGroupItem>
                                        <ListGroupItem>
                                            Price: {item.price} {item.currency} /person/night
                                        </ListGroupItem>
                                        <ListGroupItem>
                                            Payment type: {reservation.paymentType}
                                        </ListGroupItem>
                                        <ListGroupItem>
                                            Reserved with following e-mail: {reservation.email}
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

    render() {
        return(
            <div>
                {this.renderReservations()}
            </div>
        )
    }
}

export default OwnReservations;