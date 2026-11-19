export const cleanBackendMessage = (raw: any): string => {
	const msg = String(raw ?? '').trim();

	if (!msg) {
		return 'Ocurrió un error al actualizar.';
	}

	const firstLine = msg.split(/\r?\n/)[0];
	const withoutStack = firstLine.split(' at ')[0];

	const sqlExceptionIndex = withoutStack.indexOf('): ');
	if (sqlExceptionIndex !== -1) {
		return withoutStack.substring(sqlExceptionIndex + 3).trim();
	}

	const colonIndex = withoutStack.indexOf(': ');
	if (colonIndex !== -1) {
		return withoutStack.substring(colonIndex + 2).trim();
	}

	return withoutStack;
};