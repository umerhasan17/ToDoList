// this is to make sure the user is only asked once for the confirm.
var donetoday = false;
var donetomorrow = false;
var doneremember = false;
var doneweekday = false;

function DescriptionListener() {

    var description = document.getElementById('description').value;

    // this code is to get the date
    var d = new Date();
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();

    // this code is to set a maximum date for validation later on
    var maxdate;
    if (month == 2 && year % 4 == 0) {
        maxdate = 29;
    }
    else if (month == 2) {
        maxdate = 28;
    }
    else if (month == 9 || month == 4 || month == 6 || month == 11) {
        maxdate = 30;
    }
    else {
        maxdate = 31;
    }

    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";


    if (description.search("today") != -1 || description.search("Today") != -1 && donetoday == false) {
        // add due date
        donetoday = true;
        if (confirm("Add a due date for today?")) {
            document.getElementById("due-date").value = weekday[d.getDay()] + " " + day + "/" + month + "/" + year;
        }
    }
    if (description.search("tomorrow") != -1 || description.search("Tomorrow") != -1 && donetomorrow == false) {
        // add due date
        donetomorrow = true;
        if (confirm("Add a due date for tomorrow?")) {
            // essentially, if tomorrow is in the next month then change it to the next month. Don't simply increment day by 1
            if (day + 1 > maxdate) {
                month++;
                day -= maxdate;
            }
            if (month > 12) {
                month -= 12;
                year++;
            }
            document.getElementById("due-date").value = weekday[(d.getDay() + 1) & 7] + " " + (day + 1) + "/" + month + "/" + year;
        }
    }
    if (description.search("remember") != -1 || description.search("Remember") != -1 && doneremember == false) {
        // NLP Remember
        // duration set to 2 minutes
        doneremember = true;
    }
    for (var i = 0; i < 7; i++) {
        if (description.search(weekday[i]) != -1 || description.search(weekday[i].toLowerCase()) != -1 && doneweekday == false) {
            doneweekday = true;
            if (confirm("Add a due date for " + weekday[i].toLowerCase())) {
                for (var j = 1; j < 8; j++) {
                    if (weekday[i] == weekday[(d.getDay() + j) % 7]) {
                        if (day + j > maxdate) {
                            month++;
                            day -= maxdate;
                        }
                        if (month > 12) {
                            month -= 12;
                            year++;
                        }
                        document.getElementById("due-date").value = weekday[i] + " " + (day + j) + "/" + month +  "/" + year;
                    }
                }
            }
        }
    }
}