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

    deleteApprovingData = (userId) => {
        fetch("https://localhost:5000/api/users/" + userId + "/approvingdata", {
            method: 'DELETE',
            mode: "cors",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('accessToken')
            }
        })
            .then(resp => console.log(resp.status))
    }

    renderApprovingData = () => {
        return this.state.approvingData.map(
            data => {
                return (
                    <div key={data.id}>
                        {data.identityNumber === null ? null : (
                            <div>
                                <label>Identity number:</label>
                                <p>{data.identityNumber}</p>
                            </div>

                        )}

                        {data.taxNumber === null ? null : (
                            <div>
                                <label>Tax number:</label>
                                <p>{data.taxNumber}</p>
                            </div>

                        )}

                        {data.registrationNumber === null ? null : (
                            <div>
                                <label>Registration number:</label>
                                <p>{data.registrationNumber}</p>
                            </div>

                        )}
                        
                        <Button variant="outline-dark" className="mr-3">Modify</Button>
                        <Button variant="danger" onClick={() => this.deleteApprovingData(data.id)}>Delete</Button>
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