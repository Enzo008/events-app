import SearchIcon from "../../../icons/SearchIcon";

const SearchInput = ({ value, onChange }) => {
    return (
        <div className="Phone_12 flex ai-center relative" style={{position: 'relative'}}>
            <span className='flex f1_25' style={{ position: 'absolute', left: '0.5rem'}}>
                <SearchIcon />
            </span>
            <input 
                style={{paddingLeft: '2rem'}}
                className='Large_12 Large-p_5'
                type="search"
                placeholder='Buscar'
                onChange={onChange}
                value={value}
            />
        </div>
    );
}

export default SearchInput;
