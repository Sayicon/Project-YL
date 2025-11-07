/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   expand_env_utils.c                                 :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/19 16:10:11 by marcudos          #+#    #+#             */
/*   Updated: 2025/06/01 18:04:54 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

char	*extract_key(char *input, int *i)
{
	int		start;

	if (input[*i] != '$')
		return (NULL);
	(*i)++;
	start = *i;
	while (input[*i] && (ft_isalnum(input[*i]) || input[*i] == '_'))
		(*i)++;
	return (ft_substr(input, start, *i - start));
}

void	update_state_quote(t_quote *state, char c)
{
	if ((c == '\'' && *state == SINGLE_QUOTE)
		|| (c == '\"' && *state == DOUBLE_QUOTE))
		*state = NO_QUOTE;
	else if ((c == '\'' && *state == NO_QUOTE))
		*state = SINGLE_QUOTE;
	else if ((c == '\"' && *state == NO_QUOTE))
		*state = DOUBLE_QUOTE;
}

char	*find_env_value(char *key, t_env *env)
{
	while (env)
	{
		if (ft_strcmp(key, env->key) == 0)
			return (ft_strdup(env->value));
		env = env->next;
	}
	return (ft_strdup(""));
}

char	*ft_strjoin_free(char *s1, char *s2)
{
	char	*res;

	if (!s1 && !s2)
		return (NULL);
	else if (!s1)
	{
		res = ft_strdup(s2);
		free(s2);
		return (res);
	}
	else if (!s2)
	{
		res = ft_strdup(s1);
		free(s1);
		return (res);
	}
	res = ft_strjoin(s1, s2);
	free(s1);
	free(s2);
	return (res);
}
