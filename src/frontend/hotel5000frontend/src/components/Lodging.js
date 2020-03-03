import React, {Component} from 'react';

class Lodging extends Component {

    constructor(props) {
        super(props);

        this.state = {
            pageNumber: 1,
            resultPerPage: 8,
            data: {}
        }
    }

    componentDidMount() {
        this.getLodgings()
    }

    getLodgings = () => {
        let url = new URL("https://localhost:5000/api/lodgings"),
            params = {pageNumber: this.state.pageNumber, resultPerPage: this.state.resultPerPage}
            Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

        fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {

            }
        })
            .then(resp => console.log(resp.status))
    }


    render() {
        return (
            <div>

            </div>
        );
    }
}

export default Lodging;