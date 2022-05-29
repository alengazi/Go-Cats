package main

import (
	"database/sql" // database sql
	"fmt"
	"log"
	"net/http"
	"strings"

	_ "github.com/go-sql-driver/mysql"
)

var db *sql.DB

// http://localhost:8080/cats/list
func listHandler(w http.ResponseWriter, r *http.Request) {
	fmt.Print("List requested.. \n")
	rows, err := db.Query("SELECT * FROM cats_info.cats_info;")

	if err != nil {
		log.Fatal(err)
	} else {
		fmt.Fprint(w, "List of cats :) \n") 
	}
	defer rows.Close()

	for rows.Next() {
		var name string
		var description string

		if err := rows.Scan(&name, &description); err != nil {
			log.Fatal(err)
		}
		fmt.Fprintf(w, "\n %s %s\n", name, description)
	}
}

// Accessed like: http://localhost:8080/cats/create?name=Garfield&description=Cute-and-warm
func createHandler(w http.ResponseWriter, r *http.Request) { 
	catname := r.URL.Query().Get("name")
	description := r.URL.Query().Get("description")
	description = strings.ReplaceAll(description, "-", " ")

	write, err := db.Query("INSERT INTO `cats_info`.`cats_info` (`name`, `description`) VALUES (' " + catname + " ', ' " + description + " ');")

	if err != nil {
		fmt.Fprintf(w, "Creating entry.. \n")
	}
	if( write != nil) {
		fmt.Fprintf(w, "Added name-> " + catname + " & description ->" + description )
	} else {
		fmt.Fprintf(w, "\n Write failed! May have been a name that is already used.. \n ")
	}
}

func main() {
	var err error
	db, err = sql.Open("mysql", "admin:admin123@tcp(aws-test.cckilwtiu0gd.eu-north-1.rds.amazonaws.com:3306)/cats")
	if err != nil {
		fmt.Print(err.Error())
	}

	defer db.Close()

	http.HandleFunc("/cats/list", listHandler)
	http.HandleFunc("/cats/create", createHandler)
	http.ListenAndServe("localhost:8080", nil)
}
