import React, {Component} from 'react';
import {Button} from "react-bootstrap";

class ApprovingData extends Component {

    constructor(props) {
        super(props);
        this.state={
            id: null,
            approvingData: []
        }
        this._isMounted = false
    }

    componentDidMount() {
        this._isMounted = true
        this._isMounted && this.getApprovingData(this.props.id)
        this.setState({id: this.props.id})
    }

    componentWillUnmount() {
        this._isMounted = false
    }

    getApprovingData = (id) => {
        fetch("https://localhost:5000/api/users/" + id + "/approvingdata", {
            method: 'GET',
            mode: 'cors',
            headers: {}
        })
            .then(response => response.json())
            .then(responsejson => {
                this._isMounted && this.setState({approvingData: responsejson})
            })
    }

    renderApprovingData = () => {
        return this.state.approvingData.map(
            data => {
                return (
                    <div key={data.id}>
                        {data.identityNumber === null ? (
                            null
                        ) : (
                            <label>Identity number:</label>
                        )}
                        <p>{data.identityNumber}</p>

                        {data.taxNumber === null ? (
                            null
                        ) : (
                            <label>Tax number:</label>
                        )}
                        <p>{data.taxNumber}</p>

                        {data.registrationNumber === null ? (
                            null
                        ) : (
                            <label>Registration number:</label>
                        )}
                        <p>{data.registrationNumber}</p>

                        <Button variant="outline-dark" className="mr-3">Modify</Button>
                        <Button variant="danger">Delete</Button>
                    </div>
                )
            }
        )
    }
    /*TODO:confirmation required on delete button*/

    render() {
        return (
            <div>
                {this.renderApprovingData()}
            </div>
        );
    }
}

export default ApprovingData;