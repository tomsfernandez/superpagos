// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This is will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })

const withStore = fn => cy.window().its('appStore').then(x => fn(x));

Cypress.Commands.add("withStore", fn => withStore(fn));
Cypress.Commands.add("register", credentials => cy.request('POST', `${Cypress.env('EXTERNAL_API')}/api/users`, credentials));
Cypress.Commands.add("login", credentials => withStore(s => s.dispatch("login", credentials)));
Cypress.Commands.add("deleteMyself", () => withStore(s => s.dispatch("deleteMyself")));
Cypress.Commands.add("removeUser", (email) => cy.request({
    url: `${Cypress.env('EXTERNAL_API')}/api/cypress/deleteUser/${email}`,
    headers: {CYPRESS_TOKEN: Cypress.env('CYPRESS_TOKEN')}
}));
Cypress.Commands.add("logout", () => withStore(s => s.dispatch("logout")));
Cypress.Commands.add("addProvider", () => cy.request({
    url: `${Cypress.env('EXTERNAL_API')}/api/cypress/addProvider`,
    headers: {CYPRESS_TOKEN: Cypress.env('CYPRESS_TOKEN')}
}));
Cypress.Commands.add("removeProvider", () => cy.request({
    url: `${Cypress.env('EXTERNAL_API')}/api/cypress/deleteProvider`,
    headers: {CYPRESS_TOKEN: Cypress.env('CYPRESS_TOKEN')}
}));
Cypress.Commands.add("addMovements", (email) => cy.request({
    url: `${Cypress.env('EXTERNAL_API')}/api/cypress/addMovements/${email}`,
    headers: {CYPRESS_TOKEN: Cypress.env('CYPRESS_TOKEN')}
}));
Cypress.Commands.add("removeMovements", (email) => cy.request({
    url: `${Cypress.env('EXTERNAL_API')}/api/cypress/removeMovements/${email}`,
    headers: {CYPRESS_TOKEN: Cypress.env('CYPRESS_TOKEN')}
}));
Cypress.Commands.add("removeAllMethodsFromUser", (email) => cy.request({
    url: `${Cypress.env('EXTERNAL_API')}/api/cypress/deleteAllFromUser/${email}`,
    headers: {CYPRESS_TOKEN: Cypress.env('CYPRESS_TOKEN')}
}));
Cypress.Commands.add("getEmailText", () => cy.request({
    url: 'https://mailtrap.io/api/v1/inboxes/545397/messages',
    headers: {"Api-Token": "884c4fc8e348575008f086c496d520e0"}
}).then((response) => {
    let emailId = response.body[0].id;
    cy.request({
        url: `https://mailtrap.io/api/v1/inboxes/545397/messages/${emailId}/body.txt`,
        headers: {"Api-Token": "884c4fc8e348575008f086c496d520e0"}
    }).then((response) => response.body);
}));
Cypress.Commands.add("cleanInbox", () => cy.request({
    method: 'PATCH',
    url: `https://mailtrap.io/api/v1/inboxes/545397/clean`,
    headers: {"Api-Token": "884c4fc8e348575008f086c496d520e0"}
}));