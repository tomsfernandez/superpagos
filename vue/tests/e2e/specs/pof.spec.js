// type definitions for Cypress object "cy"
/// <reference types="cypress" />

const getStore = () => cy.window().its('appStore');

describe('Example Test', () => {
  it('Visits the app root url', () => {
    cy.visit('/');
    expect(getStore()).to.not.equal(undefined);
  })
});
