/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   handle_quotes.c                                    :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/06/01 18:13:40 by marcudos          #+#    #+#             */
/*   Updated: 2025/06/01 18:37:26 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

char	*rem_quotes(char *str, int free_str)
{
	char	*result;
	int		i;
	int		j;
	int		quote_handled;
	t_quote	state;

	result = malloc(ft_strlen(str) + 1);
	if (!result)
		return (NULL);
	init_vars_remove_quotes(&i, &j, &state);
	while (str[i])
	{
		quote_handled = update_state_quote_i(str, &i, &state);
		if (quote_handled)
			continue ;
		if (!((str[i] == '\'' || str[i] == '\"') && state == NO_QUOTE))
			result[j++] = str[i];
		if (str[i])
			i++;
	}
	if (free_str)
		free(str);
	result[j] = '\0';
	return (result);
}

void	init_vars_remove_quotes(int *i, int *j, t_quote *state)
{
	*i = 0;
	*j = 0;
	*state = NO_QUOTE;
}
