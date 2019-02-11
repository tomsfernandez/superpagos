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
Cypress.Commands.add("removeAllMethodsFromUser", (email) => cy.request({
  url: `${Cypress.env('EXTERNAL_API')}/api/cypress/deleteAllFromUser/${email}`, 
  headers: {CYPRESS_TOKEN: Cypress.env('CYPRESS_TOKEN')}
}));