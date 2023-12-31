const { io } = require('socket.io-client');
const mysql = require('mysql2/promise');

let db = null;
const client = io.connect('http://localhost:3001');

client.on('connect', async function(socket) {
    console.log('connected to server');

    db = await mysql.createConnection({
        host: 'localhost',
        user: 'root',
        password: 'root',
        database: 'proyecto_ntier'
    });

    console.log('connected to database');

    client.on('negocio::message', function(message) {
        console.log('message received from negocio', message);
        handleNegocioMessage(message);
    });
});

async function handleNegocioMessage(message) {
    switch (message.action) {
        case 'TransactionReason::list': {
            const [rows, fields] = await db.execute(`SELECT * FROM transaction_reasons`);
            let results = rows.map(row => ({
                id: row.id,
                type: row.type,
                reason: row.reason,
                amount: row.amount,
                employeeId: row.employee_id,
                checked: row.checked,
                createdAt: row.created_at
            }));

            results = results.map(row => {
                const date = new Date(row.createdAt);
                const year = date.getFullYear();
                const month = date.getMonth() + 1;
                const day = date.getDate();

                row.createdAt = `${year}/${month}/${day}`;
                return row;
            });

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'TransactionReason::listed',
                entity: 'TransactionReason',
                body: results.map(row => `${row.id};${row.type};${row.reason};${row.amount};${row.employeeId};${row.checked};${row.createdAt}`).join('\n')
            });
        } break;
        case 'TransactionReason::create': {
            const [_, type, reason, amount, employeeId] = message.body.split(';');
            const [rows, fields] = await db.execute(
                'INSERT INTO transaction_reasons (type, reason, amount, employee_id) VALUES (?, ?, ?, ?)', 
                [type, reason, amount, employeeId]
            );
            const id = rows.insertId;

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'TransactionReason::created',
                entity: 'TransactionReason',
                body: id
            });
        } break;
        case 'TransactionReason::update': {
            const [id, type, reason, amount, employeeId] = message.body.split(';');
            const [rows, fields] = await db.execute(`UPDATE transaction_reasons 
                SET type = ?, 
                    reason = ?,
                    amount = ?,
                    employee_id = ?
                WHERE id = ?`, [type, reason, amount, employeeId, id]);

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'TransactionReason::updated',
                entity: 'TransactionReason',
                body: rows.affectedRows
            });
        } break;
        case 'TransactionReason::delete': {
            const [id] = message.body.split(';');
            const [rows, fields] = await db.execute('DELETE FROM transaction_reasons WHERE id = ?', [id]);

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'TransactionReason::deleted',
                entity: 'TransactionReason',
                body: rows.affectedRows
            });
        } break;
        case 'Employee::add': {
            const [idCard, fullName, hireDate, salary] = message.body.split(';');
            const [rows, fields] = await db.execute('INSERT INTO employees (id_card, full_name, hire_date, salary) VALUES (?, ?, ?, ?)', [
                idCard, 
                fullName, 
                hireDate, 
                salary
            ]);
            const id = rows.insertId;

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'Employee::added',
                entity: 'Employee',
                body: id
            });
        } break;
        case 'Employee::list': {
            const [rows, fields] = await db.execute('SELECT * FROM employees');
            let results = rows.map(row => ({
                id: row.id,
                idCard: row.id_card,
                fullName: row.full_name,
                hireDate: row.hire_date,
                salary: row.salary
            }));

            results = results.map(row => {
                const date = new Date(row.hireDate);
                const year = date.getFullYear();
                const month = date.getMonth() + 1;
                const day = date.getDate();

                row.hireDate = `${year}/${month}/${day}`;
                return row;
            });
            
            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'Employee::listed',
                entity: 'Employee',
                body: results.map(row => `${row.id};${row.idCard};${row.fullName};${row.hireDate};${row.salary}`).join('\n')
            });
        } break;
        case 'Employee::update': {
            const [id, idCard, fullName, hireDate, salary] = message.body.split(';');
            const [rows, fields] = await db.execute('UPDATE employees SET id_card = ?, full_name = ?, hire_date = ?, salary = ? WHERE id = ?', [
                idCard,
                fullName,
                hireDate,
                salary,
                id
            ]);

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'Employee::updated',
                entity: 'Employee',
                body: rows.affectedRows
            });
        } break;
        case 'Employee::delete': {
            const [id] = message.body.split(';');
            const [rows, fields] = await db.execute('DELETE FROM employees WHERE id = ?', [id]);

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'Employee::deleted',
                entity: 'Employee',
                body: rows.affectedRows
            });
        } break;
        case 'AccountingEntry::add' : {
            const [creditSum, debitSum, employeeId, details] = message.body.split(';');
            const [rows, fields] = await db.execute('INSERT INTO accounting_entries (credit_sum, debit_sum, employee_id) VALUES (?, ?, ?)', [
                creditSum,
                debitSum,
                employeeId
            ]);

            const detailsMatched = details.match(/^\[([^$]+)\]$/);
            const detailsArray = detailsMatched[1].split('|');
            const detailsParsed = detailsArray.map(detail => {
                if (detail.trim() == '') return null;
                const [transactionReasonId, credit, debit] = detail.split(',');
                return { credit, debit, transactionReasonId };
            }).filter(detail => detail != null);

            const id = rows.insertId;

            for (const detail of detailsParsed) {
                const [r, _] = await db.execute('INSERT INTO accounting_entry_details (credit, debit, transaction_reason_id, accounting_entry_id) VALUES (?, ?, ?, ?)', [
                    detail.credit,
                    detail.debit,
                    detail.transactionReasonId,
                    id
                ]);

                await db.execute('UPDATE transaction_reasons SET checked = 1 WHERE id = ?', [detail.transactionReasonId]);
            }

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'AccountingEntry::added',
                entity: 'AccountingEntry',
                body: rows.affectedRows
            });
        } break;
        case 'AccountType::list': {
            const [rows, fields] = await db.execute('SELECT * FROM account_types');
            let results = rows.map(row => ({
                id: row.id,
                name: row.name,
                createdAt: row.created_at
            }));

            results = results.map(row => {
                const date = new Date(row.createdAt);

                const year = date.getFullYear();
                const month = date.getMonth() + 1;
                const day = date.getDate();

                row.createdAt = `${year}/${month}/${day}`;
                return row;
            });

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'AccountType::listed',
                entity: 'AccountType',
                body: results.map(row => `${row.id};${row.name};${row.createdAt}`).join('\n')
            });
        } break;
        case 'AccountType::create': {
            const [name] = message.body.split(';');
            const [rows, fields] = await db.execute('INSERT INTO account_types (name) VALUES (?)', [name]);
            const id = rows.insertId;

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'AccountType::created',
                entity: 'AccountType',
                body: id
            });
        } break;
        case 'AccountType::update': {
            const [id, name] = message.body.split(';');
            const [rows, fields] = await db.execute('UPDATE account_types SET name = ? WHERE id = ?', [name, id]);

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'AccountType::updated',
                entity: 'AccountType',
                body: rows.affectedRows
            });
        } break;
        case 'AccountType::delete': {
            const [id] = message.body.split(';');
            const [rows, fields] = await db.execute('DELETE FROM account_types WHERE id = ?', [id]);

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'AccountType::deleted',
                entity: 'AccountType',
                body: rows.affectedRows
            });
        } break;
        case 'Account::list': {
            const [rows, fields] = await db.execute(`
                SELECT a.id, a.name, a.created_date, t.id as account_type_id, t.name as account_type_name
                FROM accounts as a
                INNER JOIN account_types as t ON a.account_type_id = t.id
            `);

            let results = rows.map(row => ({
                id: row.id,
                name: row.name,
                createdDate: row.created_date,
                accountTypeId: row.account_type_id,
                accountTypeName: row.account_type_name
            }));

            results = results.map(row => {
                const date = new Date(row.createdDate);

                const year = date.getFullYear();
                const month = date.getMonth() + 1;
                const day = date.getDate();

                row.createdDate = `${year}/${month}/${day}`;
                return row;
            });

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'Account::listed',
                entity: 'Account',
                body: results.map(row => `${row.id};${row.name};${row.createdDate};${row.accountTypeId};${row.accountTypeName}`).join('\n')
            });
        } break;
        case 'Account::create': {
            const [name, accountTypeId] = message.body.split(';');
            const [rows, fields] = await db.execute('INSERT INTO accounts (name, account_type_id) VALUES (?, ?)', [name, accountTypeId]);
            const id = rows.insertId;

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'Account::created',
                entity: 'Account',
                body: id
            });
        } break;
        case 'Account::update': {
            const [id, name, accountTypeId] = message.body.split(';');
            const [rows, fields] = await db.execute('UPDATE accounts SET name = ?, account_type_id = ? WHERE id = ?', [name, accountTypeId, id]);

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'Account::updated',
                entity: 'Account',
                body: rows.affectedRows
            });
        } break;
        case 'Account::delete': {
            const [id] = message.body.split(';');
            const [rows, fields] = await db.execute('DELETE FROM accounts WHERE id = ?', [id]);

            client.emit('persistence::message', {
                source: 'PERSISTENCE',
                method: 'OUTPUT',
                action: 'Account::deleted',
                entity: 'Account',
                body: rows.affectedRows
            });
        } break;
    }
}