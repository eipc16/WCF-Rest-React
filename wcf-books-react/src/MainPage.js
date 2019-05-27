import React, { Component } from 'react';
import Alert from 'react-s-alert';
import XMLParser from 'react-xml-parser';
import axios from 'axios';
import format from 'xml-formatter'

import './MainStyle.css'

class SearchFilters extends Component {
    constructor() {
        super();

        this.state = {
            bookName: "",
            mounted: false
        }
        

        this.updateMode = this.updateMode.bind(this);
        this.updateBookName = this.updateBookName.bind(this);
        this.performSearch = this.performSearch.bind(this);
        this.clearFilters = this.clearFilters.bind(this);
        this.showDialog = this.showDialog.bind(this);
    }

    componentDidMount() {
        this.setState({
            mode: this.props.defaultMode,
            mounted: true
        })
    }

    updateMode(e) {
        const value = e.target.value;

        this.setState({
            mode: value
        })
    }

    updateBookName(e) {
        e.preventDefault();
        const value = e.target.value;

        this.setState({
            bookName: value
        })
    }

    performSearch(e) {
        e.preventDefault();

        this.props.onSearchPerform(this.state.mode, this.state.bookName);
    }

    clearFilters(e) {

        this.setState({
            mode: this.props.defaultMode,
            bookName: '',
            mounted: false
        }, () => {
            this.setState({
                mounted: true
            })
            this.props.onSearchPerform(this.state.mode, this.state.bookName);
        })
    }

    showDialog(e) {
        e.preventDefault()

        this.props.onShowDialog()
    }

    render() {
        if (!this.state.mounted)
            return null;

        return (
            <div className={this.props.className}>
                <p>Wybierz tryb: </p>
                <div className='mode-selector' onChange={e => this.updateMode(e)}>
                    <input type="radio" value="json" name="mode" defaultChecked={this.state.mode === 'json'} /> JSON
                    <input type="radio" value="xml" name="mode" defaultChecked={this.state.mode === 'xml'} /> XML
                </div>
                <div className='title-selector'>
                    <input type="text" value={this.state.bookName} onChange={e => this.updateBookName(e)} placeholder="Nazwa książki" />
                    <button onClick={this.clearFilters}>Resetuj</button>
                    <button onClick={this.performSearch}>Szukaj</button>
                    <button onClick={this.showDialog}>Dodaj</button>
                </div>
            </div>
        )
    }
}

class BookField extends Component {

    constructor() {
        super()

        this.state = {
            book: null
        }
    }

    componentDidMount() {
        this.setState({
            book: this.props.book
        })
    }

    removeBook(event) {
        event.preventDefault();

        this.props.onDelete(this.state.book.BookID);
    }

    showDetails(event) {
        event.preventDefault();

        this.props.onDetails(this.state.book.BookID);
    }

    render() {
        if (!this.state.book)
            return null;

        const book = this.state.book;

        return (
            <div className='book-field'>
                <p className='book-title'>Tytuł: {book.BookTitle}</p>
                <p className='book-author'>Author: {book.Author}</p>
                <p className='book-publish-year'>Wydana w roku: {book.PublishYear}</p>
                <p className='book-publisher'>Wydawnictwo: {book.Publisher}</p>

                <div className='book-action-btns'>
                    <button className='button' id='remove-button' onClick={e => this.removeBook(e)}>USUŃ</button>
                    <button className='button' id='more-button' onClick={e => this.showDetails(e)}>SZCZEGÓŁY</button>
                </div>
            </div>
        )
    }
}

class BookList extends Component {
    initList(bookData) {
        const result = [];

        for (let i = 0; i < bookData.length; i++) {
            console.log(bookData[i]);
            result.push(
                <BookField key={i} book={bookData[i]} onDelete={this.props.onDelete} onDetails={this.props.onDetails}/>
            )
        }

        return result;
    }

    render() {
        return (
            <div className={this.props.className}>
                {this.initList(this.props.bookList)} 
            </div>  
        )
    }
}

class AddBookDialog extends Component {
    constructor() {
        super()

        this.state = {
            book: {}
        }

        this.updateBookData = this.updateBookData.bind(this);
        this.closeDialog = this.closeDialog.bind(this)
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

    sendPostRequest(event) {
        event.preventDefault()
        this.props.onAddBook(this.state.book)
    }

    closeDialog(event) {
        event.preventDefault()

        this.props.onCloseDialog()
    }

    render() {
        return (
            <div className={this.props.className}>
                <p>Dodaj nową książkę</p>

                <div className="input-field">
                    <label htmlFor="BookTitle">Nazwa książki: </label>
                    <input type="text" name="BookTitle" value={this.state.book.BookTitle} onChange={this.updateBookData} />
                </div>

                <div className="input-field">
                    <label htmlFor="Author">Autor: </label>
                    <input type="text" name="Author" value={this.state.book.Author} onChange={this.updateBookData} />
                </div>

                <div className="input-field">
                    <label htmlFor="PublishYear">Rok publikacji: </label>
                    <input type="number" name="PublishYear" value={this.state.book.PublishYear} onChange={this.updateBookData} />
                </div>

                <div className="input-field">
                    <label htmlFor="Publisher">Wydawnictwo: </label>
                    <input type="text" name="Publisher" value={this.state.book.Publisher} onChange={this.updateBookData} />
                </div>

                <div className="input-field">
                    <button onClick={e => this.sendPostRequest(e)}>Dodaj książkę!</button>
                    <button className='close-button' onClick={this.closeDialog}>Zamknij</button>
                </div>
            </div>
        )
    }

}

class MainPage extends Component {
    constructor() {
        super();

        this.parser = new XMLParser();
        this.defaultMode = 'json';

        this.state = {
            mode: this.defaultMode,
            bookName: '',
            showAddDialog: false
        }

        axios.interceptors.request.use(request => {
            this.setState({
                request: JSON.stringify(request, null, 4)
            })
            return request
        })

        axios.interceptors.response.use(response => {
            this.setState({
                response: this.parseResponse(response.request.responseText)
            })
            return response
        })

        this.performSearch = this.performSearch.bind(this)
        this.removeBook = this.removeBook.bind(this)
        this.loadBookDetails = this.loadBookDetails.bind(this)
        this.showDialog = this.showDialog.bind(this)
        this.closeDialog = this.closeDialog.bind(this)
        this.addBook = this.addBook.bind(this)
    }

    parseResponse(response) {
        if (this.state.mode === 'json') {
            return JSON.stringify(JSON.parse(response), null, 4)
        } else if (this.state.mode === 'xml') {
            return format(response)
        } else {
            return "Błąd"
        }
    }

    componentDidMount() {
        this.loadBookList()
    }

    transformFromXMLObject(object) {
        let response = {}
        const children = object.children;
        for (let i = 0; i < children.length; i++) {
            let value = children[i].value

            if (children[i].children.length > 0) {
                value = this.transformFromXMLObject(children[i])
            }

            response = {
                ...response,
                [children[i].name]: value
            }
        }
        return response;
    }

    removeBook(index) {
        let url = 'http://localhost:50248/RestService.svc/' + this.state.mode + '/books/' + index

        axios.delete(url)
        .then(response => {
            Alert.success(response.data)
            this.loadBookList()
        })
        .catch(error => {
            Alert.error(error.message)

        })
    }

    loadBookDetails(index) {
        this.props.history.push('/book/' + index)
    }

    addBook(book) {
        const mode = this.state.mode

        let url = 'http://localhost:50248/RestService.svc/' + mode + '/books'

        axios.post(url, book)
        .then(response => {
            Alert.success(response.data)
            this.loadBookList()
            this.closeDialog()
        })
        .catch(error => {
            Alert.error(error.message)
        })
    }

    loadBookList() {
        const mode = this.state.mode
        const bookName = this.state.bookName

        let url = 'http://localhost:50248/RestService.svc/' + mode + '/books'
        console.log(url);
        if (bookName !== '') {
            url += '?name=' + bookName;
        }

        axios.get(url)
            .then((response) => {
                const responseData = response.data;
                let data = responseData

                if (mode === 'xml') {
                    let children = this.parser.parseFromString(responseData).children;
                    data = []

                    for (let i = 0; i < children.length; i++) {
                        data.push(this.transformFromXMLObject(children[i]))
                    }
                }

                console.log(data)

                this.setState({
                    bookList: data
                })
            })
            .catch((error) => {
                console.log(error.message)
            })
    }

    performSearch(mode, bookName) {
        this.setState({
            mode: mode,
            bookName: bookName
        }, () => {
            this.loadBookList();
        })
    }

    showDialog() {
        this.setState({
            showAddDialog: true
        })
    }

    closeDialog() {
        this.setState({
            showAddDialog: false
        })
    }

    render() {
        if (!this.state.bookList)
            return null

        return (
            <div className='main-container'>
                <SearchFilters
                    className='search-filters'
                    onSearchPerform={this.performSearch}
                    defaultMode={this.defaultMode}
                    onShowDialog={this.showDialog}/>
                <BookList
                    className='book-list'
                    bookList={this.state.bookList}
                    onDelete={this.removeBook}
                    onDetails={this.loadBookDetails}/>

                <div className='requests'>
                    {this.state.response ? (
                        <div className='response-content'>Odpowiedź:<br></br><pre>{this.state.response}</pre></div>
                    ) : (null)}

                    {this.state.request ? (
                        <div className='response-content'>Zapytanie:<br></br><pre>{this.state.request}</pre></div>
                    ) : (null)}
                </div>

                {this.state.showAddDialog ? (
                    <AddBookDialog
                        className='add-book-dialog'
                        onAddBook={this.addBook}
                        onCloseDialog={this.closeDialog} />
                ): (null)}
            </div>
        )
    }
}

export default MainPage;