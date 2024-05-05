export const validateNoLeadingSpaces = (value) => {
    if (value.startsWith(' ')) {
        return 'El campo no puede comenzar con espacios en blanco';
    }
    return true;
};