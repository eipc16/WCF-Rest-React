import React, { Component } from 'react';
import axios from 'axios';
import Alert from 'react-s-alert';

import './BookDetails.css'

class BookDetails extends Component {

    constructor() {
        super()

        this.state = {}

        this.updateBookData = this.updateBookData.bind(this);
        this.sendUpdateRequest = this.sendUpdateRequest.bind(this);
    }

    componentDidMount() {
        axios.get('http://localhost:50248/RestService.svc/json/books/' + this.props.match.params.book_id)
            .then((response) => {
                this.setState({
                    book: response.data
                })
            })
            .catch((error) => {
                Alert.error("Nie odnaleziono książki!")
                this.props.history.push("/")
            })
    }

    sendUpdateRequest(e) {
        e.preventDefault();

        const bookData = {
            BookID: this.state.book.BookID,
            BookTitle: this.state.book.BookTitle,
            Publisher: this.state.book.Publisher,
            PublishYear: this.state.book.PublishYear,
            Author: this.state.book.Author
        }

        const data = Object.assign({}, bookData)
        console.log(data);
        axios.put('http://localhost:50248/RestService.svc/json/books/' + this.props.match.params.book_id, data, {
            headers: {
                'Content-Type': 'application/json',
            }
        })
        .then((response) => {
            Alert.success(response.data);
        })
        .catch((error) => {
            Alert.error(error);
        })
    }

    updateBookData(event) {
        const name = event.target.name;
        const value = event.target.value;

        this.setState({
            book: {
                ...this.state.book,
                [name]: value
            }
        })
    }

    render() {
        if (!this.state.book)
            return null;

        return (
            <div className='update-book'>
                <div className='input-field'>
                    <label htmlFor="BookID">Identyfikator książki: </label>
                    <input type="text" name="BookID" value={this.state.book.BookID} onChange={this.updateBookData} disabled />
                </div>

                <div className='input-field'>
                    <label htmlFor="BookTitle">Nazwa książki: </label>
                    <input type="text" name="BookTitle" value={this.state.book.BookTitle} onChange={this.updateBookData} />
                </div>

                <div className='input-field'>
                    <label htmlFor="Author">Autor: </label>
                    <input type="text" name="Author" value={this.state.book.Author} onChange={this.updateBookData} />
                </div>

                <div className='input-field'>
                    <label htmlFor="PublishYear">Rok publikacji: </label>
                    <input type="number" name="PublishYear" value={this.state.book.PublishYear} onChange={this.updateBookData} />
                </div>

                <div className='input-field'>
                    <label htmlFor="Publisher">Wydawnictwo: </label>
                    <input type="text" name="Publisher" value={this.state.book.Publisher} onChange={this.updateBookData} />
                </div>

                <div className='input-field'>
                    <button onClick={this.sendUpdateRequest}>Zaktualizuj!</button>
                </div>
            </div>
        )
    }
}

export default BookDetails;