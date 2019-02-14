import faker from "faker";

describe('Password recovery test', () => {

    const credentials = {
        name: "Barney Ullrich",
        email: "Rodrick_Russel81@yahoo.com",
        password: "OAsudlZIUTckj",
        newPassword: "oaskAMFK4562"
    };

    before(() => {
        cy.server();
        cy.removeUser(credentials.email);
        cy.register(credentials);
    });

    after(() => {
        cy.removeUser(credentials.email);
    });

    // it('navigates from login page to password recovery', () => {
    //     cy.visit('/login');
    //     cy.get('[data-cy=password-recovery]').click();
    //     cy.url().should('include', 'passwordRecovery');
    // });

    // it('shows an error message if the email does not exist', () => {
    //     cy.server();
    //     cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery`, "").as('requestPasswordRecovery');
    //     cy.visit('/passwordRecovery');
    //    
    //     cy.get('[data-cy=email]').clear().type(faker.internet.email());
    //     cy.get('[data-cy=submit]').click();
    //     cy.wait('@requestPasswordRecovery');
    //     cy.get('[data-cy=error-div]').should('be.visible');
    //
    // });
    //
    // it('shows a success message when the email was sent', () => {
    //     cy.server();
    //     cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery`, "URLCALLBACK").as('requestPasswordRecovery');
    //     cy.visit('/passwordRecovery');
    //     cy.get('[data-cy=email]').clear().type(credentials.email);
    //     cy.get('[data-cy=submit]').click();
    //     cy.wait('@requestPasswordRecovery');
    //     cy.get('[data-cy=success-div]').should('be.visible');
    // });

    it('sends the email to recover the password and it navigates to the link of the new password creation', () => {
        // PROBAR ESTO, LOS OTROS ANDAN
        
        // cy.server();
        // cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery`).as('requestPasswordRecovery');
        // cy.visit('/passwordRecovery');
        // cy.get('[data-cy=email]').clear().type(credentials.email);
        // cy.get('[data-cy=submit]').click();
        // cy.wait('@requestPasswordRecovery');
        // cy.get('[data-cy=success-div]').should('be.visible');
        //
        // // cy.request('', ``);
        // const urlEndpoint = "";
        
        // cy.route({
        //     method: 'POST',
        //     url: `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery/reset`,
        // }).as('resetPassword');
        // cy.visit(urlEndpoint);
        // cy.get('[data-cy=password]').clear().type(credentials.newPassword);
        // cy.get('[data-cy=confirmedPassword]').clear().type(credentials.newPassword);
        // cy.get('[data-cy=submit]').click();
        // cy.wait('@resetPassword');
        // cy.url().should('include', '/login');
    });

    // it('shows an error message when the passwords are not equal', () => {
    //     cy.visit('/passwordRecovery/aUserId/aToken');
    //     cy.get('[data-cy=password]').clear().type("aNewPassword");
    //     cy.get('[data-cy=confirmedPassword]').clear().type("aDifferentNewPassword");
    //     cy.get('[data-cy=submit]').click();
    //     cy.get('[data-cy=error-div]').should('be.visible');
    // });
    //
    // it('redirects to login page when the new password is created', () => {
    //     cy.server();
    //     cy.route({
    //         method: 'POST',
    //         url: `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery/reset`,
    //         response: {}
    //     }).as('resetPassword');
    //     cy.visit('/passwordRecovery/aUserId/aToken');
    //     cy.get('[data-cy=password]').clear().type("aNewPassword");
    //     cy.get('[data-cy=confirmedPassword]').clear().type("aNewPassword");
    //     cy.get('[data-cy=submit]').click();
    //     cy.wait('@resetPassword');
    //     cy.url().should('include', '/login');
    // });
    //
    // it('cannot log in with the old password', () => {
    //     cy.server();
    //     cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/login`).as('login');
    //     cy.visit('/login');
    //     cy.get('[data-cy=email]').clear().type(credentials.email);
    //     cy.get('[data-cy=password]').clear().type(credentials.password);
    //     cy.get('[data-cy=submit]').click();
    //     cy.wait('@login');
    //     cy.get('[data-cy=error-div]').should('be.visible');
    //     cy.url().should('include', 'login');
    // });
    //
    // it('can log in with the new password', () => {
    //     cy.server();
    //     cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/login`).as('login');
    //     cy.visit('/login');
    //     cy.get('[data-cy=email]').clear().type(credentials.email);
    //     cy.get('[data-cy=password]').clear().type(credentials.newPassword);
    //     cy.get('[data-cy=submit]').click();
    //     cy.wait('@login');
    //     cy.url().should('include', 'home');
    // });
})
;