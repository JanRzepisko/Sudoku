document.addEventListener('DOMContentLoaded', function() {
    const table = document.getElementById('myTable');
    const form = document.getElementById('myForm');

    // Dane do wygenerowania formularza
    const data = [
        [0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0]
    ];



    // Generowanie formularza
    let a = 0
    for (let i = 0; i < data.length; i++) {
        const row = document.createElement('tr');
        for (let j = 0; j < data[i].length; j++) {
            const cell = document.createElement('td');
            const input = document.createElement('input');
            input.type = 'text';
            input.classList.add("number")
            input.name = `data[${i}][${j}]`;
            input.value = data[i][j] == 0 ? "" : data[i][j];
            input.maxLength = 1;
            input.id = `i${a}`;
            input.pattern = '[1-9]*';
            cell.appendChild(input);
            row.appendChild(cell);
            a++
        }
        table.appendChild(row);
    }


    // Wysyłanie danych na serwer
    form.addEventListener('submit', function(event) {
        event.preventDefault();

        const inputs = form.querySelectorAll('input[type="text"]');
        const updatedData = [];

        inputs.forEach(input => {
            const [i, j] = input.name.match(/\d+/g);
            const value = parseInt(input.value, 10);
            if (!isNaN(value)) {
                if (!updatedData[i]) {
                    updatedData[i] = [];
                }
                updatedData[i][j] = value;
            }
            else
            {
                if (!updatedData[i]) {
                    updatedData[i] = [];
                }
                updatedData[i][j] = 0;
            }

        });

        const formData = new FormData(form);
        const jsonData = JSON.stringify(updatedData);
        formData.append('data', jsonData);


        fetch('http://localhost:5093/', {
            method: 'post',
            mode: 'cors',
            headers: new Headers(
                {'content-type': 'application/json',
                    'Access-Control-Allow-Origin': "http://localhost"
                }),
            body: jsonData
        })
            .then(async res => {
                const response = res.json();
                console.log('Dane wysłane!');
                console.log(await response)

                let fullList = []
                let responseList = (await response)
                for(let i = 0; i < 9; i++){
                    for(let j = 0; j < 9; j++){
                        fullList.push(responseList[i][j])
                    }
                }

                for(let i = 0; i<81; i++){
                    if(inputs[i].value != fullList[i].toString())
                    {
                        document.getElementById(`i${i}`).style.backgroundColor = 'yellow';
                        inputs[i].value = fullList[i]

                        if(fullList[i] == 0){
                            document.getElementById(`i${i}`).style.backgroundColor = 'red';
                            inputs[i].value = ''
                        }
                    }
                }

            })
            .catch(error => {
                console.error('Błąd podczas wysyłania danych:', error);
            });
    });
});

