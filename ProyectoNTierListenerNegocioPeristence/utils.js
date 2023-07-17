function parseSocketResponse(response) {
	const source = response.match(/SOURCE:([^\n]+)/)[1];
	const method = response.match(/METHOD:([^\n]+)/)[1];
	const entity = response.match(/ENTITY:([^\n]+)/)[1];
	const action = response.match(/ACTION:([^\n]+)/)[1];
	const body = ((matches) => {
		if (!matches || matches.length < 2) {
			return '';
		}

		return matches[1];
	})(response.match(/BODY:([^$]+)/));

	return {
		source,
		method,
		entity,
		action,
		body
	};
}

function buildSocketMessage(source, method, entity, action, body) {
	return `SOURCE:${source}\nMETHOD:${method}\nENTITY:${entity}\nACTION:${action}\nBODY:${body}`;
}

module.exports = {
    parseSocketResponse,
    buildSocketMessage
};